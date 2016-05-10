using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour
{

    public float fov = 60;
    public float viewDistance = 15;
    private RaycastHit hit;
    public GameObject player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InLineOfSight();
    }

    public bool InLineOfSight()
    {
        Vector3 rayDirection = player.transform.position - this.transform.position;

        if (Vector3.Angle(rayDirection, this.transform.forward) <= fov)
        {
            if (Physics.Raycast(this.transform.position, rayDirection,out hit, viewDistance))
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
}
