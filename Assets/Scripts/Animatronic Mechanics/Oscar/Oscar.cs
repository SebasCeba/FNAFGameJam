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


    // Voice lines logic
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for each voice line, must match length of voiceLines array

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TeleportToRandomSpot();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            activationTimer += Time.deltaTime;
            if (activationTimer >= activationDelay)
            {
                isActive = true;
            }
            else
                return; // Skip the rest of the update until Oscar is active
        }
        if (hidden)
        {
            timer += Time.deltaTime;
            if(timer >= timeToFind)
            {
                if(powerSystem != null)
                {
                    powerSystem.Power -= powerDrain;
                }
                Recover();
            }
        }
    }
    void TeleportToRandomSpot()
    {
        int idx = Random.Range(0, hideSpots.Length);
        transform.position = hideSpots[idx].position;
        currentArea = areaManagers[idx]; //Set the current area
        SetLookDirection(currentArea.lookEulerAngles); // Face the correct direction
        hidden = true;
        timer = 0f; 
    }
    public void TryShock(AreaManager area)
    {
        if(area == currentArea)
        {
            Recover();
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
    public void Recover()
    {
        hidden = false;
        timer = 0f;
        TeleportToRandomSpot();
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
