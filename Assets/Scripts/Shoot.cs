using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	
	public Rigidbody projectile;
	public float speed = 20;
		
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("I am trying to fire");
			Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(90,0,0)) as Rigidbody;
			
			instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0,speed));
			
		}
	}
}