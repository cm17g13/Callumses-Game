using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	
	public GameObject projectile;
	public GameObject status;
	public Material enabled;
	public Material disabled;
	public float speed = 40F;
    public float damage = 5F;
	private float time = 1F;
	public float shotTime = 1F;
		
	// Use this for initialization
	void Start () {
		
	}

	public void newSpeed() {
		this.speed *= 2F;
	}

	public void newDamage() {
		this.damage *= 2F;
	}

	public void newShotTime() {
		this.shotTime *= 0.5F;
	}
	
	// Update is called once per frame
	void Update () {
		
		time  = time + Time.deltaTime;
		//Debug.Log(time);
		if (time > shotTime) {
			status.GetComponent<Renderer>().material = enabled; 
			if (Input.GetMouseButtonDown (0)) {

                GameObject obj = Instantiate(projectile, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
                obj.GetComponent<BulletScript>().setDamage(damage);
                Rigidbody instantiatedProjectile = obj.GetComponent<Rigidbody>();
                instantiatedProjectile.velocity = transform.TransformDirection (new Vector3 (0, 0, speed));
				status.GetComponent<Renderer>().material = disabled;
				time = 0;
			}
		}
	}
}