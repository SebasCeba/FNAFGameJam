using Artemis;
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
    public float windowIdleTime = 8f; // Time spent idling at the window

    public float activationDelay = 3f; // Delay before shane starts moving 

    private int currentPoint = 0;
    private bool observing = false;
    private float observeTimer = 0f;
    private bool activated = false;
    private float activationTimer = 0f;
    private bool atEndAction = false;
    private bool windowIdle = false;
    private float windowIdleTimer = 0f;

    public FPController player;
    public FPLookController playerLook; 
    private NavMeshAgent agent;
    public Door officeDoor; //   Reference to the door script
    public JumpscareManager jsManager; // Reference to the Jumpscare Manager
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
                GoToRandomPoint();
            }
            else
            {
                return; // Not activated yet
            }
        }
        // Shane stands at the window for a set time 
        if (windowIdle)
        {
            windowIdleTimer += Time.deltaTime;
            transform.LookAt(windowPoint);
            if (windowIdleTimer >= windowIdleTime)
            {
                windowIdle = false;
                windowIdleTimer = 0f;
                // After window idle, randomly choose next target 
                ChooseDynamicAction();
            }
            return;
        }
        if (observing)
        {
            observeTimer += Time.deltaTime; 
            transform.LookAt(playerPoint);
            if(observeTimer >= observeTime)
            {
                observing = false;
                GoToRandomPoint();
            }
            return; 
        }
        // Shane is at the end of his path and is performing an action
        if (atEndAction)
        {
            // Shane is preforming his end action
            if(!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                // If at the door, check door state 
                if(agent.destination == doorPoint.position && officeDoor != null)
                {
                    if (!officeDoor.IsOpen)
                    {
                        // Door is closed, return to start 
                        GoToRandomPoint();
                        atEndAction = false;
                        return;
                    }
                    else
                    {
                        // Door is open, observe player 
                        observing = true;
                        observeTimer = 0f;
                        atEndAction = false;
                        return;
                    }
                }
                else if(agent.destination == playerPoint.position)
                {
                    // Attack the player
                    if (playerLook != null)
                    {
                        playerLook.canLook = false; // Disable looking
                    }
                    if(jsManager != null)
                    {
                        jsManager.TriggerJumpscare("Shane");
                    }
                    GoToRandomPoint();
                    atEndAction = false;
                }
                else
                {
                    // Finished end action, resume patrol 
                    GoToRandomPoint();
                    atEndAction = false;
                }
            }
            return; 
        }
        // Patrol logic 
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            ChooseDynamicAction();
        }
    }
    private void GoToRandomPoint()
    {
        if(wanderPoints.Length == 0)
            return;
        currentPoint = Random.Range(0, wanderPoints.Length);
        agent.SetDestination(wanderPoints[currentPoint].position);
    }
    private void ChooseDynamicAction()
    {
        // 0 : Random patrol, 1: Window peek, 2: Door, 3: Attack player 
        int action = Random.Range(0, 4);

        switch (action)
        {
            case 0: // Go to a random patrol point  
                GoToRandomPoint();
                break;
            case 1: // Peek through window
                agent.SetDestination(windowPoint.position);
                windowIdle = true; // Start window idle
                windowIdleTime = 0f;
                atEndAction = false; // windowIdle will handle next step 
                break;
            case 2: // Go to door 
                agent.SetDestination(doorPoint.position);
                atEndAction = true; 
                break;
            case 3: // Attack player
                agent.SetDestination(playerPoint.position);
                atEndAction = true;
                break;
        }
    }
    // Collider trigger for game over 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Shane OnTriggerEnter: hit {other.name} with tag {other.tag}");
        if (other.CompareTag("Office"))
        {
            Debug.Log("Office door is closed. Triggering jumpscare and showing options panel.");
            if (officeDoor != null && !officeDoor.IsOpen)
            {
                //Door is closed, game over 
                if(jsManager != null)
                {
                    jsManager.TriggerJumpscare(AnimatronicType.Shane);
                }
            }
        }
    }
}
