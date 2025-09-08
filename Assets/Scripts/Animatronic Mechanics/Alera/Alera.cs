using Artemis;
using UnityEngine;

public class Alera : MonoBehaviour
{
    public Player player;

    public float activationDelay = 30f;
    private bool isActive = false; 
    private float activationTimer = 0f;

    public float cameraTimeSpent = 0f;
    public float cameraTimeThreshold = 60f;
    public bool inOffice = false;

    // Voice lines logic
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for each voice line, must match length of voiceLines array

    private void Update()
    {
        if (!isActive)
        {
            activationTimer += Time.deltaTime;
            if(activationTimer >= activationDelay)
            {
                isActive = true;
            }
            else
                return; // Skip the rest of the update until Alera is active
        }
        if(player != null && player.camManager != null && player.camManager.CamerasOpen)
        {
            cameraTimeSpent += Time.deltaTime; 
        }
        if(!inOffice && cameraTimeSpent > cameraTimeThreshold)
        {
            AppearInOffice();
        }
    }
    void AppearInOffice()
    {
        inOffice = true;
        // Move to office position, play jumpscare 
        //player.Defeat(); 
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
