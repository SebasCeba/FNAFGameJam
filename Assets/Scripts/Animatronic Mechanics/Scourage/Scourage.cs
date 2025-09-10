using Artemis;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Scourage : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public Transform doorTransform; // Reference to the door's transform
    public Transform originTransform; // Where they came from 
    public float moveSpeed = 5f; // Speed at which Scourage moves
    public bool charging = false; // Whether Scourage is currently charging
    public bool returning = false; // Whether Scourage is returning to origin
    public bool hasAttackedPlayer = false; // Prevent multiple triggers

    private bool activated = false;
    private float activationTimer = 0f;
    public float activationDelay = 3f; // Delay before shane starts moving 

    private NavMeshAgent agent;
    public PowerSystem powerSystem; // Reference to the power system
    public CameraManager cameraManager; // Reference to the camera manager
    [SerializeField] public float powerTaken; // Power drained when door is closed

    public JumpscareManager jsManager; // Reference to the Jumpscare Manager
    public float chargeTimeOut = 5f; // Time before giving up charge
    private float chargeTimer = 0f; // Timer for charge timeout
    private float waitTimer = 0f; // Timer for waiting between charges
    public float nextChargeDelay = 20f; // Fixed delay before next charge  
    private bool waitingForNextCharge = false; //  Whether waiting for next charge
    public float cameraInfluenceFactor = 0.5f; // Factor to reduce wait time if cameras are open

    // Voice lines logic 
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for each voice line, must match length of voiceLines array
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }
    private void Update()
    {
        if(!activated)
        {
            activationTimer += Time.deltaTime;
            if (activationTimer >= activationDelay)
            {
                activated = true;
                StartCharge();
            }
            else
            {
                return; // Not activated yet
            }
        }
        if(hasAttackedPlayer)
        {
            agent.isStopped = true; // Stop moving after attack
            return;
        }
        if (charging)
        {
            agent.isStopped = false; // Ensure agent is moving
            agent.SetDestination(playerTransform.position);
            chargeTimer += Time.deltaTime;

            // If scourage reaches the player attack point
            if (Vector3.Distance(transform.position, playerTransform.position) < 0.5f)
            {
                charging = false;
                hasAttackedPlayer = true; // Prevent multiple triggers
                agent.isStopped = true; // Stop moving

                if (jsManager != null)
                {
                    jsManager.TriggerJumpscare(AnimatronicType.Scourage);
                }
                else
                {
                    Debug.LogWarning("JumpscareManager reference is missing on Scourage.");
                }
                return;
            }
            // If scourage fails to reach the player in time
            if(chargeTimer >= chargeTimeOut)
            {
                charging = false;
                returning = true;
                chargeTimer = 0f;
                DrainPower(); // Drain power when charge fails
            }
        }
        else if(returning)
        {
            // Instantly teleport back to origin for soft reset 
            agent.isStopped = true; // Ensure agent is moving
            transform.position = originTransform.position;
            returning = false;
            waitingForNextCharge = true;
            waitTimer = 0f;
            //agent.SetDestination(originTransform.position);

            // Face the player 
            if (playerTransform != null)
            {
                Vector3 lookDirection = (playerTransform.position - transform.position).normalized;
                lookDirection.y = 0f; // Keep only horizontal rotation
                if (lookDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(lookDirection);
                }
            }
            //if (Vector3.Distance(transform.position, originTransform.position) < 0.5f)
            //{
            //    returning = false;
            //    waitingForNextCharge = true;
            //    waitTimer = 0f; // Reset activation timer for next charge

            //}
        }
        else if (waitingForNextCharge)
        {
            waitTimer += Time.deltaTime;
            Debug.Log("Scourage waiting to charge...");
            if(cameraManager != null && cameraManager.CamerasOpen)
            {
                // If cameras are open, decrease the amount of time that scourage can attack again 
                nextChargeDelay -= cameraInfluenceFactor * Time.deltaTime;
                if (nextChargeDelay < 1f) nextChargeDelay = 1f; // Clamp to minimum of 1 second
            }
            if (waitTimer >= nextChargeDelay)
            {
                waitingForNextCharge = false;
                waitTimer = 0f;
                nextChargeDelay = 20f; // Reset to default
                StartCharge(); 
            }
        }
    }
    public void StartCharge()
    {
        charging = true; 
        hasAttackedPlayer = false; // Reset attack state
        chargeTimer = 0f;
        agent.isStopped = false;
    }
    public void DrainPower()
    {
        // Only drain power if the door is closed 
        if (powerSystem != null)
        {
            powerSystem.Power -= powerTaken;
            if(powerSystem.Power < 0f)
            {
                powerSystem.Power = 0f; // Clamp to zero
            }
            Debug.Log($"Scourage drained {powerTaken} power! Current power: {powerSystem.Power}%");
        }
    }
    public AudioClip GetRandomVoiceLine()
    {
        float totalWeight = 0f;
        foreach (var w in voiceLineWeights)
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
