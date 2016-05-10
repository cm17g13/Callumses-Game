using UnityEngine;
using System.Collections;

public class BotBulletScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bot" || collision.gameObject.tag == "BotBullet")
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
