using Unity.VisualScripting;
using UnityEngine;

public class Oscar : MonoBehaviour
{
    [Header("Hide Spots and Area Management")]
    public Transform[] hideSpots;
    public AreaManager currentArea; // The area oscar is currently hiding in
    public AreaManager[] areaManagers; // All area managers in the scene

    [Header("Activation Settings & Timing")]
    public float activationDelay = 30f; // Time in seconds before Oscar starts teleporting
    private bool isActive = false;
    private int attemptCount = 0; // Number of attempts player has made to shock Oscar
    public int maxAttempts = 3; // Max attempts before Oscar teleports immediately
    private Vector3 spawnPos; // Initial spawn position
    private Quaternion spawnRot; // Initial spawn rotation
    private float activationTimer = 0f; // Timer to track activation delay
  
    public float timeToFind = 20f; // Time in seconds to find Oscar
    private float timer = 0f;
    private bool hidden = false;

    [Header("Power System Settings & Penalties")]
    public PowerSystem powerSystem;
    public float powerDrain = 10f;
    public float teleportInterval = 30f;
    public float timePenalty = 5f;
    public float extraPowerDrain = 5f;

    [Header("Oscar Appearance Settings")]
    private Vector3 originalScale;
    private Vector3 originalPosition; 
    private int lastAreaIndex = -1; // To avoid teleporting to the same area consecutively

    [Header("Zap Drainage Settings")]
    private bool inVent = false;
    public float ventDrainrate = 5f; // Power drain rate when in vent
    public float ventAttackDelay = 5f; // Time before Oscar can attack in vent
    private float ventTimer = 0f;

    [Header("Vent Settings")]
    public Transform ventPoint; // Assign in inspector

    // Reference 
    public JumpscareManager jumpscareManager; 
    // Voice lines logic
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for each voice line, must match length of voiceLines array

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
        spawnPos = transform.position;
        spawnRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Activation Timer 
        if(!isActive)
        {
            activationTimer += Time.deltaTime;
            if (activationTimer >= activationDelay)
            {
                isActive = true;
                TeleportToRandomSpot();
                hidden = true; // Start hidden
                timer = 0f; // Reset timer
                attemptCount = 0; // Reset attempts
            }
            return; // Skip the rest of the update until Oscar is active
        }
        // 2. vent logic 
        if (inVent)
        {
            // Drain power over time while in vent
            if(powerSystem != null)
            {
                powerSystem.Power -= ventDrainrate * Time.deltaTime;
            }
            ventTimer += Time.deltaTime; 
            if(ventTimer >= ventAttackDelay)
            {
                // Attack logic here 
                jumpscareManager.TriggerJumpscare(AnimatronicType.Oscar);
                inVent = false; // Reset vent state after attack
            }
            return; // Skip the rest of the update while in vent
        }
        // 3. Player must find oscar bnefore timers runs out 
        if (hidden)
        {
            timer += Time.deltaTime;
            if(timer >= timeToFind)
            {
                attemptCount++;
                if(attemptCount < maxAttempts)
                {
                    // Drain power and reset timer for another attempt 
                    if(powerSystem != null)
                    {
                        powerSystem.Power -= powerDrain;
                    }
                    TeleportToRandomSpot();
                    timer = 0f; // Reset timer
                }
                else
                {
                    // After two failed attempts, teleport to vent
                    EnterVent();
                }
            }
        }
    }
    void TeleportToRandomSpot()
    {
        int idx = Random.Range(0, hideSpots.Length);
        lastAreaIndex = idx; // Tracks for zap recovery 
        transform.position = hideSpots[idx].position;
        currentArea = areaManagers[idx]; //Set the current area
        SetLookDirection(currentArea.lookEulerAngles); // Face the correct direction

        // Crouch logic 
        if (currentArea.isCrouchArea)
        {
            Vector3 crouchScale = originalScale;
            crouchScale.y *= 0.5f; // Crouch by halving the height
            transform.localScale = crouchScale;
        }
        else
        {
            transform.localScale = originalScale; // Reset to original scale
        }

        hidden = true;
        timer = 0f; 
    }
    public void TryShock(AreaManager area)
    {
        if(area == currentArea && hidden)
        {
            RecoverToSpawn();
        }
        else
        {
            // Might just lower amount of time needed to find oscar or increase the power the player loses
            timer += timePenalty;
            if (timer > timeToFind)
            {
                timer = timeToFind; // Cap the timer to not exceed timeToFind
            }
            // Drain additional power (same as powerDrain) 
            if(powerSystem != null)
            {
                powerSystem.Power -= extraPowerDrain;
            }
        }
    }
    private void EnterVent()
    {
        // Teleport oscar to the vent point 
        if(ventPoint != null)
        {
            transform.position = ventPoint.position;
        }

        // Crouch logic: halve Oscar's Y Scale
        Vector3 crouchScale = originalScale;
        crouchScale.y *= 0.5f; // Crouch by halving the height
        transform.localScale = crouchScale;

        // Start vent state and timer 
        inVent = true;
        ventTimer = 0f;
        hidden = false; // Oscar is no longer hidden in a spot
    }
    public void RecoverToSpawn()
    {
        hidden = false;
        timer = 0f;
        attemptCount = 0; // Reset attempts
        isActive = false; // Deactivate Oscar until next activation
        inVent = false; // Ensure vent state is reset
        // Return oscar to original position and rotation
        transform.position = spawnPos;
        transform.rotation = spawnRot;
        transform.localScale = originalScale; // Reset scale
        activationTimer = 0f; // Reset activation timer
    }
    public void SetLookDirection(Vector3 eulerAngles)
    {
        transform.rotation = Quaternion.Euler(eulerAngles);
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
