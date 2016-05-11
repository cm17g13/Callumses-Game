using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Patrolling : MonoBehaviour {
    public Transform[] patrolPoints;

    protected int currentPatrolPoint = -1;
    protected int patrolPointModifier = 1;

    protected NavMeshAgent agent;

    public float fov = 60;
    public float viewDistance = 15;
    private RaycastHit hit;
    public GameObject player;

    public Rigidbody projectile;
    public float speed = 20;
    public float time = 1;
    public float shotTime = 1;
    public bool isDetected = false;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
	}

    Transform GetNextPoint()
    {
        currentPatrolPoint += patrolPointModifier;
        if (currentPatrolPoint >= patrolPoints.Length || currentPatrolPoint < 0)
        {
            patrolPointModifier = -patrolPointModifier;
            currentPatrolPoint += 2 * patrolPointModifier;
        }
        return patrolPoints[currentPatrolPoint];
    }

    void GoToNextPoint()
    {
        if(patrolPoints.Length == 0) { return; }
        agent.destination = GetNextPoint().position;
        
    }

    void GoToPlayer()
    {
        agent.destination = player.transform.position;
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
                    Debug.Log("Can see player");
                    return true;
                }
            }
        }
        return false;
    }

    public bool Detected()
    {
        return isDetected;
    }

    // Update is called once per frame
    void Update () {

        if(InLineOfSight()) {
            GoToPlayer();
            this.isDetected = true;
            time = time + Time.deltaTime;
            if (time > shotTime) {
                shoot();
                time = 0;
            }
        } else if (agent.remainingDistance < 1f) {
            this.isDetected = false;
            GoToNextPoint();
            time = 0;
        }	
	}
}
