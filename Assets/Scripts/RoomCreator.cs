using UnityEngine;
using System.Collections;

public class RoomCreator : MonoBehaviour {
    public GameObject wallBlock;
    public Vector3 origin;
    public float cellSizeInWorldUnits = 3;
    public float wallThickness = 0.1f;
    public float wallHeight = 3f;

    private Vector3 northWallOffset;
    private Vector3 westWallOffset;
    private RoomGenerator generator = new RoomGenerator(10);

	// Use this for initialization
	void Start () {
        calculateOffsets();
        generator.createMap();
        spawnRoom();
	}

    void calculateOffsets()
    {
        northWallOffset = new Vector3(0, wallHeight/2, cellSizeInWorldUnits / 2);
        westWallOffset = new Vector3(-cellSizeInWorldUnits / 2, wallHeight/2, 0);
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
        if ((generator.map[x, y].walls & Cell.Walls.North) == Cell.Walls.North)
        {
            createWall(x, y, Cell.Walls.North);
        }
        if ((generator.map[x, y].walls & Cell.Walls.West) == Cell.Walls.West)
        {
            createWall(x, y, Cell.Walls.West);
        }
    }

    GameObject createWall(int x, int y, Cell.Walls wallDirection)
    {
        Vector3 wallDimensions = new Vector3(cellSizeInWorldUnits, wallHeight, wallThickness);
        Vector3 offset = (wallDirection == Cell.Walls.North) ? northWallOffset : westWallOffset;
        Quaternion rotation = (wallDirection == Cell.Walls.North) ? Quaternion.identity : Quaternion.Euler(0, 90, 0);
        Vector3 position = gridToWorldPosition(x, y) + offset;

        GameObject wall = (GameObject)Instantiate(wallBlock, position, rotation);
        wall.transform.localScale = wallDimensions;
        return wall;
    }
}
