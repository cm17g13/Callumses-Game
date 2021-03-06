﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Patrolling : MonoBehaviour {
    public List<Transform> patrolPoints;

    protected int currentPatrolPoint = -1;
    protected int patrolPointModifier = 1;

    protected NavMeshAgent agent;

    public float health = 10;
    public float fov = 60;
    public float viewDistance = 15;
    private RaycastHit hit;
    private GameObject player;

    public Rigidbody projectile;
    public float speed = 20;
    public float time = 1;
    public float shotTime = 1;
    [SerializeField]
    public State state = State.Passive;
    public bool chasing = false;

    public enum State { Detected = 3, Alerted = 2, Passive = 1}

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
	}

    Transform GetNextPoint()
    {
        currentPatrolPoint += patrolPointModifier;
        if (currentPatrolPoint >= patrolPoints.Count || currentPatrolPoint < 0)
        {
            patrolPointModifier = -patrolPointModifier;
            currentPatrolPoint += 2 * patrolPointModifier;
        }
        return patrolPoints[currentPatrolPoint];
    }

    void GoToNextPoint()
    {
        if(patrolPoints.Count == 0) { return; }
        agent.destination = GetNextPoint().position;
        
    }

    void GoToPlayer()
    {
        agent.destination = player.transform.position;
        chasing = true;
    }

    void shoot()
    {
        Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(90, 0, 0)) as Rigidbody;
        Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), this.GetComponent<Collider>());
        instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, speed));      
     
    }

    public bool InLineOfSight()
    {
        Vector3 rayDirection = player.transform.position - this.transform.position;

        if (Vector3.Angle(rayDirection, this.transform.forward) <= fov)
        {
            if (Physics.Raycast(this.transform.position, rayDirection, out hit, viewDistance))
            {
                if (hit.transform.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool Detected()
    {
        return state == State.Detected;
    }

    public void Hit(float damage)
    {
		BecomeAlert ();
        Debug.Log(this.health);
        this.health -= damage;
        Debug.Log(this.health);
        if (this.health <= 0)
        {
            Destroy(gameObject);
        } 
    }

	public void BecomeAlert() {
		this.state = State.Alerted;
		GoToPlayer ();
	}

    // Update is called once per frame
    void Update () {

        bool canSee = InLineOfSight();
        if (canSee)
        {
            GoToPlayer();
            this.state = State.Detected;
            time = time + Time.deltaTime;
            if (time > shotTime)
            {
                shoot();
                time = 0;
            }
        }
        else if(chasing)
        {
            this.state = State.Alerted;
        }

		if (!canSee && agent.remainingDistance < 1f && !agent.pathPending) {
            chasing = false;
            this.state = State.Passive;
            time = 0;
			GoToNextPoint();
        }	
	}
}
