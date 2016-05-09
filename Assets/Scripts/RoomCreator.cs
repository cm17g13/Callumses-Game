using UnityEngine;
using System.Collections;

public class RoomCreator : MonoBehaviour {
    public Vector3 origin;
    public float cellSizeInWorldUnits = 1;
    public float wallThickness = 0.1f;
    public float wallHeight = 3f;

    private RoomGenerator generator = new RoomGenerator(10);

	// Use this for initialization
	void Start () {
        generator.createMap();
        generator.mapSize = 2;
        generator.map = new RoomGenerator.GridState[2,2] { { RoomGenerator.GridState.NorthWall, RoomGenerator.GridState.NorthWall}, { RoomGenerator.GridState.WestWall, RoomGenerator.GridState.WestWall} };
        spawnRoom();
	}

    Vector3 gridToWorldPosition(int x, int y)
    {
        return origin + new Vector3(y * cellSizeInWorldUnits, 0, -x * cellSizeInWorldUnits);
    }

    void spawnRoom()
    {
        for(int x = 0; x < generator.map.GetLength(0); x++)
        {
            for(int y = 0; y < generator.map.GetLength(1); y++)
            {
                spawnCell(x, y);
            }
        }
    }

    void spawnCell(int x, int y)
    {
        createWall(x, y, generator.map[x, y]);
    }

    GameObject createWall(int x, int y, RoomGenerator.GridState wallDirection)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if ((wallDirection & RoomGenerator.GridState.NorthWall) == RoomGenerator.GridState.NorthWall) {
            Vector3 wallDimensions = new Vector3(cellSizeInWorldUnits, wallHeight, wallThickness);
            wall.transform.localScale = wallDimensions;
            wall.transform.position = gridToWorldPosition(x, y) + new Vector3(0, 0, -cellSizeInWorldUnits / 2);
        } else
        {
            Vector3 wallDimensions = new Vector3(wallThickness, wallHeight, cellSizeInWorldUnits);
            wall.transform.localScale = wallDimensions;
            wall.transform.position = gridToWorldPosition(x, y) + new Vector3(-cellSizeInWorldUnits / 2, 0, 0);
        }
        return wall;
    }
}
