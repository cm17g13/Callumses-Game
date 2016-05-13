using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RoomCreator : MonoBehaviour {
    public GameObject wallBlock;
    public GameObject patrolBot;
    public GameObject objectivePrefab;
    public Vector3 origin;
    public float cellSizeInWorldUnits = 3;
    public float wallThickness = 0.1f;
    public float wallHeight = 3f;

    private Dictionary<Dir, Vector3> offsets;
    private RoomGenerator generator;

    public void performCreation(RoomGenerator newGenerator)
    {
        generator = newGenerator;
        calculateOffsets();
        spawnArea();
        spawnEnemies();
        spawnObjectives();
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

    public Vector3 gridToWorldPosition(int x, int y)
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

    void spawnEnemies()
    {
        foreach (PatrolRoute route in generator.patrols)
        {
            GameObject bot = spawnBot();
            AI_Patrolling botAi = bot.GetComponent<AI_Patrolling>();
            botAi.patrolPoints = new List<Transform>();
            foreach (Cell point in route.points)
            {
                GameObject patrolPoint = new GameObject("Patrol Point");
                patrolPoint.transform.position = gridToWorldPosition(point.x, point.y);
                botAi.patrolPoints.Add(patrolPoint.transform);
            }
            int patrolStart = UnityEngine.Random.value < 0.5f ? 0 : 1;
            bot.transform.position = botAi.patrolPoints[patrolStart].transform.position;
        }
    }

    GameObject spawnBot()
    {
        return Instantiate(patrolBot);
    }

    void spawnObjectives()
    {
        foreach(Cell objectiveLoc in generator.objectives)
        {
            Vector3 objectivePos = gridToWorldPosition(objectiveLoc.x, objectiveLoc.y);
            objectivePos.y = origin.y;
            Instantiate(objectivePrefab, gridToWorldPosition(objectiveLoc.x, objectiveLoc.y), Quaternion.identity);
        }
    }

}
