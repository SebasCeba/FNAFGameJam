using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Editor;

public class AnimatronicAI : MonoBehaviour
{
    public Transform[] waypoints;
    //public Transform securityRoom;
    protected NavMeshAgent agent;
    protected int currentWaypointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextWayPoint();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            ActOnArrival();
        }
    }
    protected virtual void ActOnArrival()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            GoToNextWayPoint();
        }
    }
    protected void GoToNextWayPoint()
    {
        agent.SetDestination(waypoints[currentWaypointIndex].position); 
        currentWaypointIndex++;
    }
}
