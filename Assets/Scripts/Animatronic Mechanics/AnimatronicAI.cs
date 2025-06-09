using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Editor;

public class AnimatronicAI : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform securityRoom;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (currentWaypointIndex < waypoints.Length)
            {
                GoToNextWayPoint();
            }
            else
            {
                agent.SetDestination(securityRoom.position);
            }
        }
    }
    void GoToNextWayPoint()
    {
        agent.SetDestination(waypoints[currentWaypointIndex].position); 
        currentWaypointIndex++;
    }
}
