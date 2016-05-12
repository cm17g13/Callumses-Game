using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

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
			collision.gameObject.GetComponent<RigidbodyFirstPersonController>().Death();
        }
        Destroy(gameObject);
    }
}
