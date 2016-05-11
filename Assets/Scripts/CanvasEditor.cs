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
        int count = 0;
        foreach (var obj in objects)
        {
            count++;
            //ScriptName sn = 
            //obj.GetComponent<AI_Patrolling>().Detected();
            if (obj.GetComponent<AI_Patrolling>().Detected())
            {
                text.text = "Detected";
                text.color = Color.red;
                count = 0;
            }
        }
        if (count == objectCount)
        {
            text.text = "Concealed";
            text.color = Color.grey;
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
