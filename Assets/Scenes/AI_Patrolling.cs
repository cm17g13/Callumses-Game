using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Patrolling : MonoBehaviour {
    public Transform[] patrolPoints;

    protected int currentPatrolPoint = -1;
    protected int patrolPointModifier = 1;


    protected NavMeshAgent agent;

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
	
	// Update is called once per frame
	void Update () {
        if(agent.remainingDistance < 1f)
        {
            GoToNextPoint();
        }	
	}
}
