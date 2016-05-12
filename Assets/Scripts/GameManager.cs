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
		upgradedPlayer = Upgrade(GameObject player);
		upgradedPlayer.transform.position = calculateSpawnPosition();
    }

    Vector3 calculateSpawnPosition()
    {
        return creator.gridToWorldPosition(generator.spawnCell.x, generator.spawnCell.y);
    }

    public void NextLevel()
    {
        Application.LoadLevel("Game");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void GameObject Upgrade(GameObject player) {
		upgradeNumber = Random.Range (0, 4);
		subPlayer = player.gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
		subplayer.GetComponent<SpawnPoint>();
		switch(upgradeNumber)
		{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
		}
		return player;
	}

}
