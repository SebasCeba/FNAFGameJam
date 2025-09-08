using UnityEngine;

public class Oscar : MonoBehaviour
{
    public Transform[] hideSpots;
    public float activationDelay = 30f; // Time in seconds before Oscar starts teleporting
    private bool isActive = false;
    private float activationTimer = 0f; // Timer to track activation delay

    public float powerDrain = 10f;
    public float timeToFind = 20f; // Time in seconds to find Oscar
    private float timer = 0f;
    private bool hidden = false;

    public PowerSystem powerSystem;
    public AreaManager currentArea; // Not implemented yet
    public float teleportInterval = 30f;

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
        //timer += Time.deltaTime;
        //if (timer >= teleportInterval)
        //{
        //    TeleportToRandomSpot();
        //    timer = 0f;
        //}
    }
    void TeleportToRandomSpot()
    {
        int idx = Random.Range(0, hideSpots.Length);
        transform.position = hideSpots[idx].position;
        hidden = true;
        timer = 0f; 

        // Face a direction 
        //Vector3 lookDir = (Vector3.zero - transform.position).normalized;
        //if(lookDir != Vector3.zero)
        //    transform.rotation = Quaternion.LookRotation(lookDir);
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
        }
    }
    public void Recover()
    {
        hidden = false;
        timer = 0f;
        TeleportToRandomSpot();
    }
    public void SetLookDirection(bool left)
    {
        if (left)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f); // Look left
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f); // Look right
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
