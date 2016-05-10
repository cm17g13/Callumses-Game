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

public class Cell
{
    public int roomId = 0;
    public bool isAssigned = false;
    public int x, y;
}

public class RoomGenerator {
    public int maxCorridorLength = 5;

    public Cell[,] map;
    public int mapSize;

    private int currentRoomId = 0;

    public RoomGenerator(int newMapSize)
    {
        mapSize = newMapSize;
        map = new Cell[mapSize, mapSize];
        forEachCell((x, y) => map[x, y] = new Cell() { x = x, y = y });
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

    //Creates a room by assigning it in the map and allocating north and west walls.
    //Should work, despite not allocation south and east, as long as all squares allocated.
    public void createRoom(Rect room)
    {
        currentRoomId++;

        int xMin = (int)room.x;
        int yMin = (int)room.y;
        int xMax = (int)room.xMax - 1;
        int yMax = (int)room.yMax - 1;

        for(int x = xMin; x <= xMax; x++)
        {
            for(int y = yMin; y <= yMax; y++)
            {
                map[x, y].roomId = currentRoomId;
                map[x, y].isAssigned = true;
            }
        }
    }

    public void useTestData()
    {
        mapSize = 2;
        map = new Cell[2, 2] { { new Cell() { x = 0, y = 0, roomId = 1 }, new Cell() { x = 0, y = 1, roomId = 1 } }, { new Cell() { x = 1, y = 0, roomId = 2 }, new Cell() { x = 1, y = 1, roomId = 2 } } };
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

    public void createMap()
    {
        Rect objectiveRoom = new Rect(2, 2, 5, 5);
        createRoom(objectiveRoom);
        spawnCorridors();
        connectCorridors();
    }

    private void connectCorridors()
    {
        forEachCell((x, y) =>
        {
            int adjacentRooms = getAdjacentRoomCount(x, y);
        });
    }

    private void spawnCorridors()
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
        foreach (Dir dir in Enum.GetValues(typeof(Dir)))
        {
            Cell adjacentCell = getCellInDirection(x, y, dir);
            count = (adjacentCell != null && adjacentCell.roomId == currentCell.roomId) ? count : count + 1;
        }
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
        int desiredLength = UnityEngine.Random.Range(1, maxCorridorLength);
        int actualLength = getMaxLengthInDirection(location.x, location.y, corridorDirection, desiredLength);

        int x = corridorDirection == Dir.West ? location.x - (actualLength - 1) : location.x;
        int y = corridorDirection == Dir.North ? location.y - (actualLength - 1) : location.y;
        int width = corridorDirection == Dir.North || corridorDirection == Dir.South ? 1 : actualLength;
        int height = corridorDirection == Dir.North || corridorDirection == Dir.South ? actualLength : 1;

        //Debug.Log("Creating Corridor " + x + " " + y + " " + width + " " + height);
        createRoom(new Rect(x, y, width, height));
    }
}
