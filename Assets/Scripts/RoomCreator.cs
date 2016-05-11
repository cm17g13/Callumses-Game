using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RoomCreator : MonoBehaviour {
    public GameObject wallBlock;
    public Vector3 origin;
    public float cellSizeInWorldUnits = 3;
    public float wallThickness = 0.1f;
    public float wallHeight = 3f;
    public int worldSize = 10;

    private Dictionary<Dir, Vector3> offsets;
    private RoomGenerator generator;

	// Use this for initialization
	void Start () {
        generator = new RoomGenerator(worldSize);
        generator.createMap();
        calculateOffsets();
        spawnArea();
    }

    void calculateOffsets()
    {
        offsets = new Dictionary<Dir, Vector3>
        {
            { Dir.North, new Vector3(0, wallHeight / 2, cellSizeInWorldUnits / 2) },
            { Dir.West, new Vector3(-cellSizeInWorldUnits / 2, wallHeight/2, 0) },
            { Dir.South, new Vector3(0, wallHeight / 2, -cellSizeInWorldUnits / 2) },
            { Dir.East, new Vector3(cellSizeInWorldUnits / 2, wallHeight/2, 0) }
        };
    }

    Vector3 gridToWorldPosition(int x, int y)
    {
        return origin + new Vector3(x * cellSizeInWorldUnits, 0, -y * cellSizeInWorldUnits);
    }

    void spawnArea()
    {
        spawnAreaWalls();
        foreach (Boundry boundry in generator.boundries)
        {
            if (boundry.type == Boundry.Type.wall)
            {
                spawnBoundry(boundry);
            }
        }
    }

    void spawnAreaWalls()
    {
        int xMax = generator.map.GetLength(0) - 1;
        int yMax = generator.map.GetLength(1) - 1;
        for(int y = 0; y <= yMax; y++)
        {
            createWall(0, y, Dir.West);
            createWall(xMax, y, Dir.East);
        }
        for(int x = 0; x <= xMax; x++)
        {
            createWall(x, 0, Dir.North);
            createWall(x, yMax, Dir.South);
        }
    }

    void spawnBoundry(Boundry boundry)
    {
        createWall(boundry.cell1.x, boundry.cell1.y, boundry.dir);
    }

    GameObject createWall(int x, int y, Dir wallDirection)
    {
        Vector3 wallDimensions = new Vector3(cellSizeInWorldUnits, wallHeight, wallThickness);
        Vector3 offset = offsets[wallDirection];
        Quaternion rotation = (wallDirection == Dir.North || wallDirection == Dir.South) ? Quaternion.identity : Quaternion.Euler(0, 90, 0);
        Vector3 position = gridToWorldPosition(x, y) + offset;

        GameObject wall = (GameObject)Instantiate(wallBlock, position, rotation);
        wall.transform.localScale = wallDimensions;
        return wall;
    }
}
