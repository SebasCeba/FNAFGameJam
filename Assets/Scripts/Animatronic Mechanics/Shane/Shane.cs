using Artemis;
using UnityEngine;
using UnityEngine.AI;

public class Shane : MonoBehaviour
{
    [Header("Waypoints & Targets")]
    public Transform[] wanderPoints;
    public Transform windowPoint;
    public Transform playerPoint;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Behavior Timers")]
    public float activationDelay = 3f; // Delay before shane starts moving 
    public float windowIdleTime = 8f; // Time spent idling at the window
    public float observeTime = 5f;

    [Header("References")]
    public FPController player;
    public FPLookController playerLook;
    public Door officeDoor; //   Reference to the door script
    public JumpscareManager jsManager; // Reference to the Jumpscare Manager

    [Header("Voice Lines")]
    // Voice lines logic 
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for each voice line, must match length of voiceLines array

    // Internal state
    private NavMeshAgent agent;
    private int currentPoint = 0;
    private float observeTimer = 0f;  
    private float activationTimer = 0f;
    private float windowIdleTimer = 0f;
    private bool observing = false;
    private bool activated = false;
    private bool atEndAction = false;
    private bool windowIdle = false;
    private bool hasAttackedPlayer = false; // Prevent multiple triggers 

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
            if(activationTimer >= activationDelay)
            {
                activated = true;
                GoToRandomPoint();
            }
            else
            {
                return; // Not activated yet
            }
        }
        // Check for attack on player target 
        if(!hasAttackedPlayer && playerPoint != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerPoint.position);
            if(distanceToPlayer < 1.0f && atEndAction) // Adjust range as needed 
            {
                // Check door state before attacking 
                if (officeDoor != null && !officeDoor.IsOpen)
                {
                    // Door is closed, cannot attack and resume patrol 
                    GoToRandomPoint();
                    atEndAction = false;
                    return;
                }
                agent.isStopped = true; // Stop moving
                hasAttackedPlayer = true; // Prevent multiple triggers
                Debug.Log("Shane reached player target! Trigger jumpscare."); 
                if(jsManager != null)
                {
                    jsManager.TriggerJumpscare(AnimatronicType.Shane);
                }
                return;
            }
        }
        // If Shane has attacked, do nothing else 
        if(hasAttackedPlayer)
            return;
        // Shane stands at the window for a set time 
        if(windowIdle)
        {
            windowIdleTimer += Time.deltaTime;
            transform.LookAt(windowPoint);
            if (windowIdleTimer >= windowIdleTime)
            {
                windowIdle = false;
                windowIdleTimer = 0f;
                // After window idle, randomly choose to attack player or patrol 
                if(Random.value < 0.5f)
                {
                    agent.SetDestination(playerPoint.position);
                    atEndAction = true; // After reaching player, resume patrol
                }
                else
                {
                    GoToRandomPoint();
                }
            }
            return;
        }
        if(observing)
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
            case 2: // Attack player
                agent.SetDestination(playerPoint.position);
                atEndAction = true;
                break;
        }
    }
    public AudioClip GetRandomVoiceLine()
    {
        float totalWeight = 0f;
        foreach(var w in voiceLineWeights)
        {
            totalWeight += w;
        }
        float randomValue = Random.Range(0f, totalWeight);
        float accum = 0f;
        for (int i = 0; i < voiceLines.Length; i++)
        {
            accum += voiceLineWeights[i];
            if (randomValue <= accum)
            {
                return voiceLines[i];
            }
        }
        return voiceLines.Length > 0 ? voiceLines[0] : null; // Fallback
    }
}
