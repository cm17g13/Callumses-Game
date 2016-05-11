using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasEditor : MonoBehaviour {

    Text text;

    void Detected()
    {
        text = GetComponent<Text>();
        var objects = GameObject.FindGameObjectsWithTag("Bot");
        var objectCount = objects.Length;
        AI_Patrolling.State highestRecordedState = AI_Patrolling.State.Passive;

        foreach (var obj in objects)
        {
            if(obj.GetComponent<AI_Patrolling>().state > highestRecordedState)
            {
                highestRecordedState = obj.GetComponent<AI_Patrolling>().state;
            }

        }

        switch(highestRecordedState)
        {
            case AI_Patrolling.State.Detected:
                text.text = "Detected";
                text.color = Color.red;
                break;
            case AI_Patrolling.State.Alerted:
                text.text = "Alerted";
                text.color = Color.yellow;
                break;
            case AI_Patrolling.State.Passive:
                text.text = "Concealed";
                text.color = Color.grey;
                break;
        }
    }


	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.text = "Concealed";
        text.color = Color.grey;
    }
	
	// Update is called once per frame
	void Update () {
        Detected();
	}
}
