using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {
    public GameObject player;
    public RoomGenerator generator;
    public RoomCreator creator;
	public Text upgradeText;


	GameObject Upgrade(GameObject newPlayer) {
		int upgradeNumber = Random.Range (0, 4);
		GameObject textThing = GameObject.FindGameObjectWithTag("UpgradeText");
		upgradeText = textThing.GetComponent<Text>();
		//GameObject subPlayer = newPlayer.gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
		switch(upgradeNumber)
		{
		case 0: 
			newPlayer.GetComponent<RigidbodyFirstPersonController>().newForwardSpeed();
			upgradeText.text = (upgradeText.text + "\n" + "You have a movement speed upgrade");
			break;
		case 1:
			newPlayer.GetComponent<RigidbodyFirstPersonController>().newJumpForce();
			upgradeText.text = (upgradeText.text + "\n" + "You have a jump upgrade");
			break;
		case 2:
			newPlayer.GetComponentInChildren<Shoot>().newSpeed();
			upgradeText.text = (upgradeText.text + "\n" + "You have a bullet speed upgrade");
			break;
		case 3:
			newPlayer.GetComponentInChildren<Shoot>().newDamage();
			upgradeText.text = (upgradeText.text + "\n" + "You have a bullet damage upgrade");
			break;
		case 4:
			newPlayer.GetComponentInChildren<Shoot>().newShotTime();
			upgradeText.text = (upgradeText.text + "\n" + "You have a shot speed upgrade");
			break;
		}
		Debug.Log(upgradeText);
		return newPlayer;
	}

    // Use this for initialization
    void Start() {
        generator.haltonSequence(5, 2);
        player = GameObject.FindGameObjectWithTag("Player");

        generator.createMap();
        creator.performCreation(generator);
		GameObject upgradedPlayer = Upgrade(player);
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

    public void OnObjectiveComplete(Objective objective)
    {
        Debug.Log("Objective accomplished");
        Application.LoadLevel("Game");
    }
	
	// Update is called once per frame
	void Update () {
	
	}



}
