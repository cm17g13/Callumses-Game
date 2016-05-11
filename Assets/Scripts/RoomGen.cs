using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum Dir
{
    None,
    North,
    East,
    South,
    West
}

public class Room
{
    public HashSet<Room> adjacentRooms = new HashSet<Room>();
    public List<Cell> cells = new List<Cell>();

    public int accessPortalCount = 0;
    public int extraPortalCount = 0;
}

public class Cell
{
    public Room room = null;
    public bool isAssigned { get { return room != null; } }
    public List<Dir> transitions = new List<Dir>();
    public int x, y;
}

public class Boundry
{
    public Cell cell1, cell2;
    public Room room1 { get { return cell1.room;  } }
    public Room room2 { get { return cell2.room; } }
    public Dir dir;
    public Type type = Type.wall;

    public enum Type
    {
        portal,
        wall
    }

    public Boundry(Cell newCell1, Cell newCell2, Dir newDir)
    {
        cell1 = newCell1;
        cell2 = newCell2;
        dir = newDir;
    }

    public bool Involves(Room room)
    {
        return cell1.room == room || cell2.room == room;
    }

    public Room OtherRoom(Room room)
    {
        return (room == cell1.room) ? cell2.room : cell1.room;
    }
}

public class RoomGenerator {
    public int minCorridorLength = 2;
    public int maxCorridorLength = 4;
    public int maxAccessPortals = 4;
    public int maxExtraPortals = 2;

    public List<Boundry> boundries;
    public List<Room> rooms;
    public Cell[,] map;
    public int mapSize;

    public RoomGenerator(int newMapSize)
    {
        mapSize = newMapSize;
    }

    public delegate void GridIterCallback(int x, int y);
    public void forEachCell(GridIterCallback callback)
    {
        for(int x = 0; x < mapSize; x++)
        {
            for(int y = 0; y < mapSize; y++)
            {
                callback(x, y);
            }
        }
    }

    public delegate void AdjacentCellCallback(Cell adjacentCell, Dir dir);
    public void forEachAdjacentCell(Cell currentCell, AdjacentCellCallback callback)
    {
        foreach (Dir dir in Enum.GetValues(typeof(Dir)))
        {
            if(dir == Dir.None) { continue; }
            Cell adjacentCell = getCellInDirection(currentCell.x, currentCell.y, dir);
            if(adjacentCell == null) { continue; }
            callback(adjacentCell, dir);
        }
    }

    public void init()
    {
        boundries = new List<Boundry>();
        rooms = new List<Room>();
        map = new Cell[mapSize, mapSize];
        forEachCell((x, y) => map[x, y] = new Cell() { x = x, y = y });
    }

    public void createMap()
    {
        init();
        Rect objectiveRoom = new Rect(2, 2, 4, 4);
        createRoom(objectiveRoom);
        createCorridors();

        determineAdjacentRooms();
        calculateBoundries();

        //connectCorridors();
        connectCorridors2();
    }

    private void calculateBoundries()
    {
        forEachCell((x, y) =>
        {
            Cell currentCell = map[x, y];
            Cell eastCell = getCellInDirection(x, y, Dir.East);
            Cell southCell = getCellInDirection(x, y, Dir.South);
            if(eastCell != null && eastCell.room != currentCell.room)
            {
                boundries.Add(new Boundry(currentCell, eastCell, Dir.East));
            }
            if (southCell != null && southCell.room != currentCell.room)
            {
                boundries.Add(new Boundry(currentCell, southCell, Dir.South));
            }
        });
    }

    //Creates a room by assigning it in the map and allocating north and west walls.
    //Should work, despite not allocation south and east, as long as all squares allocated.
    public void createRoom(Rect dimensions)
    {

        int xMin = (int)dimensions.x;
        int yMin = (int)dimensions.y;
        int xMax = (int)dimensions.xMax - 1;
        int yMax = (int)dimensions.yMax - 1;

        Room room = new Room();
        rooms.Add(room);

        for(int x = xMin; x <= xMax; x++)
        {
            for(int y = yMin; y <= yMax; y++)
            {
                map[x, y].room = room;
                room.cells.Add(map[x, y]);
            }
        }
    }

    public void useTestData()
    {
        mapSize = 2;
        Room roomOne = new Room();
        Room roomTwo = new Room();
        map = new Cell[2, 2] { { new Cell() { x = 0, y = 0, room = roomOne }, new Cell() { x = 0, y = 1, room = roomOne } }, { new Cell() { x = 1, y = 0, room = roomTwo }, new Cell() { x = 1, y = 1, room = roomTwo } } };
    }
    
    public List<Cell> getUnassignedSquares()
    {
        List<Cell> squares = new List<Cell>();
        forEachCell((x, y) =>
        {
            if(!map[x,y].isAssigned)
            {
                squares.Add(map[x,y]);
            }
        });
        return squares;
    }

    private void determineAdjacentRooms()
    {
        forEachCell((x, y) =>
        {
            Cell currentCell = map[x, y];
            forEachAdjacentCell(currentCell, (adjacentCell, dir) =>
            {
                if(adjacentCell != null && adjacentCell.room != currentCell.room)
                {
                    currentCell.room.adjacentRooms.Add(adjacentCell.room);
                    adjacentCell.room.adjacentRooms.Add(currentCell.room);
                }
            });
        });
    }

    private void connectCorridors2()
    {
        //Shuffle boundries
        for (int i = 0; i < boundries.Count; i++)
        {
            int swapPosition = UnityEngine.Random.Range(0, boundries.Count);
            Boundry temp = boundries[swapPosition];
            boundries[swapPosition] = boundries[i];
            boundries[i] = temp;
        }

        HashSet<Room> accessibleRooms = new HashSet<Room>();
        accessibleRooms.Add(rooms[0]);

        bool boundryAdded = true;
        while(accessibleRooms.Count < rooms.Count)
        {
            boundryAdded = false;
            foreach (Boundry boundry in boundries)
            {
                Room room = (accessibleRooms.Contains(boundry.cell1.room))? boundry.cell1.room : ((accessibleRooms.Contains(boundry.cell2.room))? boundry.cell2.room : null);
                if(room != null) {
                    Room otherRoom = boundry.OtherRoom(room);
                    if (room.accessPortalCount < maxAccessPortals && otherRoom.accessPortalCount < maxAccessPortals && !accessibleRooms.Contains(otherRoom))
                    {
                        boundry.type = Boundry.Type.portal;
                        accessibleRooms.Add(otherRoom);
                        room.accessPortalCount++;
                        otherRoom.accessPortalCount++;
                        boundryAdded = true;
                        //break;
                    }
                    
                    if (room.extraPortalCount < maxExtraPortals && otherRoom.extraPortalCount < maxExtraPortals)
                    {
                        boundry.type = Boundry.Type.portal;
                        accessibleRooms.Add(otherRoom);
                        room.extraPortalCount++;
                        otherRoom.extraPortalCount++;
                        boundryAdded = true;
                        //break;
                    }
                }
            }
            if(!boundryAdded)
            {
                Debug.Log("Boundry failed to be found, aborting");
                break;
            }
        }
    }

    private void createCorridors()
    {
        List<Cell> unassigned = getUnassignedSquares();

        int lastCount = unassigned.Count;
        int iterationsNoCountChange = 0;
        int abortThreshold = 3;

        while (unassigned.Count > 0 && iterationsNoCountChange < abortThreshold)
        {
            iterationsNoCountChange = unassigned.Count >= lastCount ? iterationsNoCountChange + 1 : 0;

            spawnCorridor(unassigned[UnityEngine.Random.Range(0, unassigned.Count-1)]);
            unassigned = getUnassignedSquares();
        }

        if(iterationsNoCountChange >= abortThreshold)
        {
            Debug.LogError("Error: Aborted Corridor Spawning, number of grid cells free not changing");
        }
    }

    public Cell getCellInDirection(int x, int y, Dir dir)
    {
        int xOffset = 0, yOffset = 0;
        switch (dir)
        {
            case Dir.North:
                yOffset = -1;
                break;
            case Dir.South:
                yOffset = 1;
                break;
            case Dir.East:
                xOffset = 1;
                break;
            case Dir.West:
                xOffset = -1;
                break;
            case Dir.None:
                return null;
        }
        int xTest = x + xOffset;
        int yTest = y + yOffset;
        if (xTest < 0 || xTest >= map.GetLength(0) || yTest < 0 || yTest >= map.GetLength(1)) { return null; };
        return map[xTest, yTest];
    }

    public int getAdjacentRoomCount(int x, int y)
    {
        int count = 0;
        Cell currentCell = map[x, y];
        forEachAdjacentCell(currentCell, (adjacentCell, dir) =>
        {
            count = (adjacentCell != null && adjacentCell.room == currentCell.room) ? count : count + 1;
        });
        return count;
    }

    private bool isDirectionFree(int x, int y, Dir dir) {
        Cell possibleCell = getCellInDirection(x, y, dir);
        return possibleCell != null && !possibleCell.isAssigned;
    }

    private Dir getArbitraryFreeDirection(int x, int y)
    {
        Dir[] dirs = (Dir[])Enum.GetValues(typeof(Dir));
        for(int i = 0; i < dirs.Length; i++)
        {
            int swapPosition = UnityEngine.Random.Range(0, dirs.Length);
            Dir temp = dirs[swapPosition];
            dirs[swapPosition] = dirs[i];
            dirs[i] = temp;
        }
        foreach (Dir dir in dirs)
        {
            if (isDirectionFree(x, y, dir)) return dir;
        }
        return Dir.None;
    }

    private int getMaxLengthInDirection(int x, int y, Dir dir, int limit)
    {

        Cell currentCell = map[x, y];
        for (int len = 1; len <= limit; len++)
        {
            if (isDirectionFree(currentCell.x, currentCell.y, dir))
            {
                currentCell = getCellInDirection(currentCell.x, currentCell.y, dir);
            }
            else
            {
                return len;
            }
        }
        return limit;
    }

    private void spawnCorridor(Cell location)
    {
        Dir corridorDirection = getArbitraryFreeDirection(location.x, location.y);
        int desiredLength = UnityEngine.Random.Range(minCorridorLength, maxCorridorLength);
        int actualLength = getMaxLengthInDirection(location.x, location.y, corridorDirection, desiredLength);

        int x = corridorDirection == Dir.West ? location.x - (actualLength - 1) : location.x;
        int y = corridorDirection == Dir.North ? location.y - (actualLength - 1) : location.y;
        int width = corridorDirection == Dir.North || corridorDirection == Dir.South ? 1 : actualLength;
        int height = corridorDirection == Dir.North || corridorDirection == Dir.South ? actualLength : 1;

        //Debug.Log("Creating Corridor " + x + " " + y + " " + width + " " + height);
        createRoom(new Rect(x, y, width, height));
    }
}
