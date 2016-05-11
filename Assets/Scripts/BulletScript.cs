using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public int count = 0;
    public int bounceLimit = 3;
    public float damage = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setDamage(float newDamage)
    {
        this.damage = newDamage;
    }

    void OnCollisionEnter(Collision collision)
    {
        count++;
        if (collision.gameObject.tag == "BotBullet")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);

        } else if (collision.gameObject.tag == "Bot")
        {
            collision.gameObject.GetComponent<AI_Patrolling>().Hit(damage);
            Destroy(gameObject);

        } else {
            if (count >= bounceLimit)
            {
                Destroy(gameObject);
            }
        }
    }
}
