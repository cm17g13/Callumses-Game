using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {
    public int triggerRadius = 2;

    private int triggerRadiusSquared;
    private GameObject player;
    private GameManager gameManager;

    private bool triggered = false;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        triggerRadiusSquared = triggerRadius * triggerRadius;
	}
	
	// Update is called once per frame
	void Update () {
        bool inTriggerRadius = (transform.position - player.transform.position).sqrMagnitude < triggerRadiusSquared;
        if (!triggered && inTriggerRadius) 
        {
            triggered = true;
            gameManager.OnObjectiveComplete(this);
        } else if(!inTriggerRadius)
        {
            triggered = false;
        }
	}
}
