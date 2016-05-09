using UnityEngine;
using System.Collections;
using System;

public class RoomGen : MonoBehaviour {
    [Flags]
    public enum GridState : int
    {
        Clear = 0,
        NorthWall = 1,
        WestWall = 2,
        RoomAssigned = 4
    }

    protected GridState[,] roomMap;
    protected int mapSize;

    public void init(int newMapSize)
    {
        mapSize = newMapSize;
        roomMap = new GridState[mapSize, mapSize];
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
                roomMap[x, y] = GridState.Clear;
                if (x == room.x)
                {
                    roomMap[x, y] |= GridState.NorthWall;
                }
                //Handle adding South wall;
                if (x == xHighestCoord) { }
                if( y == room.y)
                {
                    roomMap[x, y] |= GridState.WestWall;
                }
                //Handle adding East wall
                if (y == yHighestCoord) { }
                roomMap[x, y] |= GridState.RoomAssigned;
            }
        }
    }

    public void createMap()
    {

    }
}
