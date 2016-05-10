﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RoomCreator : MonoBehaviour {
    public GameObject wallBlock;
    public Vector3 origin;
    public float cellSizeInWorldUnits = 3;
    public float wallThickness = 0.1f;
    public float wallHeight = 3f;

    private Dictionary<Dir, Vector3> offsets;
    private RoomGenerator generator = new RoomGenerator(10);

	// Use this for initialization
	void Start () {
        calculateOffsets();
        generator.createMap();
        generator.useTestData();
        spawnRoom();
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
        Cell currentCell = generator.map[x, y];

        foreach(Dir dir in Enum.GetValues(typeof(Dir)))
        {
            Cell adjacentCell = generator.getCellInDirection(x, y, dir);
            Debug.Log("XY " + x + " " + y + " " + adjacentCell + " " + dir);
            if (dir != Dir.None && (adjacentCell == null || adjacentCell.roomId != currentCell.roomId))
            {
                Debug.Log("Spawning wall with dir " + dir);
                if (adjacentCell != null) { Debug.Log("Cell data " + adjacentCell.x + " " + adjacentCell.y + " " + adjacentCell.roomId); }
                createWall(x, y, dir);
            }
        }
    }

    GameObject createWall(int x, int y, Dir wallDirection)
    {
        Vector3 wallDimensions = new Vector3(cellSizeInWorldUnits, wallHeight, wallThickness);
        Vector3 offset = offsets[wallDirection];
        Quaternion rotation = (wallDirection == Dir.North || wallDirection == Dir.South) ? Quaternion.identity : Quaternion.Euler(0, 90, 0);
        Vector3 position = gridToWorldPosition(x, y) + offset;
        Debug.Log("Wall created with " + x + " " + y + " " + position);

        GameObject wall = (GameObject)Instantiate(wallBlock, position, rotation);
        wall.transform.localScale = wallDimensions;
        return wall;
    }
}
