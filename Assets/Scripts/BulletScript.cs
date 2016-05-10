using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public int count = 0;
    public int bounceLimit = 2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        count++;
        if(collision.gameObject.tag == "Bot" || collision.gameObject.tag == "BotBullet")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        } else {
            if (count >= bounceLimit)
            {
                Destroy(gameObject);
            }
        }
    }
}
