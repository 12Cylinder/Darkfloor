using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour {

    public GameObject primarytarget;
    public GameObject secondaryTarget;
    public int IdleRefreshTime = 5; //how often to find new idle points if no player is found
    public float aggressionRange = 10;

    private NavMeshAgent agent;
    private float lastCycleTime;
    private SphereCollider trigger;
    [System.Serializable]
    public enum AIState
    {
        idle,
        chasing,
        attacking
    }

	void Start ()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(0,0,0));
        trigger = this.GetComponent<SphereCollider>();
        trigger.radius = aggressionRange;
	}

    private void Update()
    {
        if(primarytarget != null)
        {
            agent.SetDestination(primarytarget.transform.position);
        }
        else if(secondaryTarget != null)
        {
            primarytarget = secondaryTarget;
        }
        else
        {
            if(Time.time > lastCycleTime + IdleRefreshTime)
            {
                int newX = Random.Range(-10, 10);
                int newZ = Random.Range(-10, 10);
                Vector3 newPos = new Vector3(transform.position.x + newX, 0, transform.position.z + newZ);
                NavMeshHit hit;
                if(NavMesh.SamplePosition(newPos, out hit, 2.0f, NavMesh.AllAreas))
                {
                    if (hit.hit)
                    {
                        agent.SetDestination(new Vector3(transform.position.x + newX, 0, transform.position.z + newZ));
                        lastCycleTime = Time.time;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!primarytarget)
            {
                primarytarget = other.gameObject;
            }
            else if(!secondaryTarget)
            {
                secondaryTarget = other.gameObject;
            }
        }
    }
}
