using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public GameObject player;
    public RoomGenerator generator;
    public RoomCreator creator;


    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        generator.createMap();
        creator.performCreation(generator);

        player.transform.position = calculateSpawnPosition();
    }

    Vector3 calculateSpawnPosition()
    {
        return creator.gridToWorldPosition(generator.spawnCell.x, generator.spawnCell.y);
    }

    public void NextLevel()
    {
        Application.LoadLevel("Game");
    }

    public void OnObjectiveComplete(Objective objective)
    {
        Debug.Log("Objective accomplished");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
