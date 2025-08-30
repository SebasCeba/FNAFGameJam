using Artemis;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Shane : MonoBehaviour
{
    public Transform[] wanderPoints;
    public Transform windowPoint;
    public Transform doorPoint;
    public Transform playerPoint;
    public float moveSpeed = 3f;
    public float observeTime = 5f;

    public float activationDelay = 3f; // Delay before shane starts moving 

    private int currentPoint = 0;
    private bool observing = false;
    private float observeTimer = 0f;
    private bool activated = false;
    private float activationTimer = 0f;
    private bool atEndAction = false; 

    public FPController player;
    private NavMeshAgent agent;
    public Door officeDoor; //   Reference to the door script
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updatePosition = true;
        agent.updateRotation = true; // We handle rotation manually
    }
    private void Update()
    {
        if(!activated)
        {
            activationTimer += Time.deltaTime;
            if (activationTimer >= activationDelay)
            {
                activated = true;
                GoToNextPoint();
            }
            else
            {
                return; // Not activated yet
            }
        }
        if (observing)
        {
            observeTimer += Time.deltaTime; 
            transform.LookAt(playerPoint);
            if(observeTimer >= observeTime)
            {
                observing = false;
                GoToNextPoint();
            }
            return; 
        }
        if (atEndAction)
        {
            // Shane is preforming his end action
            if(!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                atEndAction = false;
                currentPoint = 0; // Reset to first point
                GoToNextPoint();
            }
            return; 
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint++;
            if (currentPoint >= wanderPoints.Length)
            {
                ChooseEndAction();
            }
            else
            {
                Transform target = wanderPoints[currentPoint];
                agent.SetDestination(target.position);
                if (target == windowPoint || target == doorPoint)
                {
                    observing = true;
                    observeTimer = 0f;
                }
                if(target == doorPoint)
                {
                    observing = true;
                    observeTimer = 0f; 

                    // Check if the door is closed 
                   if(officeDoor != null && !officeDoor.IsOpen)
                   {
                        // Player is safe, Shane stops here
                        observing = false;
                        currentPoint = 0; // Reset to first point
                        agent.SetDestination(wanderPoints[0].position);
                        atEndAction = false;
                        return;
                   }
                }
            }
        }
    }
    private void GoToNextPoint()
    {
        if(wanderPoints.Length == 0)
            return;
        agent.destination = wanderPoints[currentPoint].position;
    }
    private void ChooseEndAction()
    {
        // Randomly pick one of three actions 
        int action = Random.Range(0, 3);
        atEndAction = true;

        switch (action)
        {
            case 0: // Attack player 
                agent.SetDestination(playerPoint.position);
                break;
            case 1: // Peek through window
                agent.SetDestination(windowPoint.position);
                break;
            case 2: // return to original spot
                agent.SetDestination(wanderPoints[0].position);
                break;

        }
    }
}
