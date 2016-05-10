using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Cell
{
    [Flags]
    public enum Walls : int
    {
        None = 0,
        North = 1,
        West = 2,
    }

    public bool isAssigned = false;
    public Walls walls = Walls.None;
}

public class RoomGenerator {
    public Cell[,] map;
    public int mapSize;

    public RoomGenerator(int newMapSize)
    {
        mapSize = newMapSize;
        map = new Cell[mapSize, mapSize];
        forEachCell((x, y) => map[x, y] = new Cell());
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
        int xStart = (int)room.x;
        int yStart = (int)room.y;
        int xHighestCoord = (int)room.xMax - 1;
        int yHighestCoord = (int)room.yMax - 1;

        for(int x = xStart; x < xHighestCoord; x++)
        {
            for(int y = yStart; y < yHighestCoord; y++)
            {
                map[x, y] = new Cell();
                if (x == room.x)
                {
                    map[x, y].walls |= Cell.Walls.North;
                }
                //Handle adding South wall;
                if (x == xHighestCoord) { }
                if( y == room.y)
                {
                    map[x, y].walls |= Cell.Walls.West;
                }
                //Handle adding East wall
                if (y == yHighestCoord) { }
                map[x, y].isAssigned = true;
            }
        }
    }

    public void useTestData()
    {
        mapSize = 2;
        map = new Cell[2,2] { { new Cell() { walls = Cell.Walls.North }, new Cell() { walls = Cell.Walls.North } }, { new Cell() { walls = Cell.Walls.North | Cell.Walls.West }, new Cell() { walls = Cell.Walls.None } } };
    }
    /*
    public List<Coords> unassignedSquares()
    {
        List<Coords> squares = new List<Coords>();
        forEachCell((x, y) =>
        {
            if((map[x, y] & Walls.RoomAssigned) == Walls.Clear)
            {
                squares.Add(new Coords(x, y));
            }
        });
        return squares;
    }*/

    public void createMap()
    {
        Rect objectiveRoom = new Rect(2, 2, 5, 5);
        createRoom(objectiveRoom);
    }
}
