using UnityEngine;
using System.Collections;
using System;

public class RoomGenerator {
    [Flags]
    public enum GridState : int
    {
        Clear = 0,
        NorthWall = 1,
        WestWall = 2,
        RoomAssigned = 4
    }

    public GridState[,] map;
    public int mapSize;

    public RoomGenerator(int newMapSize)
    {
        mapSize = newMapSize;
        map = new GridState[mapSize, mapSize];
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
                map[x, y] = GridState.Clear;
                if (x == room.x)
                {
                    map[x, y] |= GridState.NorthWall;
                }
                //Handle adding South wall;
                if (x == xHighestCoord) { }
                if( y == room.y)
                {
                    map[x, y] |= GridState.WestWall;
                }
                //Handle adding East wall
                if (y == yHighestCoord) { }
                map[x, y] |= GridState.RoomAssigned;
            }
        }
    }

    public void useTestData()
    {
        mapSize = 2;
        map = new RoomGenerator.GridState[2,2] { { RoomGenerator.GridState.NorthWall, RoomGenerator.GridState.NorthWall}, { RoomGenerator.GridState.WestWall | RoomGenerator.GridState.NorthWall, RoomGenerator.GridState.Clear} };
    }

    public void createMap()
    {
        Rect objectiveRoom = new Rect(2, 2, 5, 5);
        createRoom(objectiveRoom);
    }
}
