using UnityEngine;

public class Amelia : MonoBehaviour
{
    public Material[] moodMaterials; // If any case the modeler doesn't include the materials/variants for happy, warning and danger moods.
    public SkinnedMeshRenderer meshRenderer; // Reference to the SkinnedMeshRenderer component
    public Animator animator; // Reference to the Animator component
    public float meterMax = 100f; // Maximum value of the meter
    public float meterValue = 100f; // Current value of the meter
    public float meterDecayRate = 5f; // Rate at which the meter decays per second

    public float activationDelay = 30f; // Time in seconds before Amelia starts to decay
    private bool isActive = false;
    private float activationTimer = 0f; // Timer to track activation delay

    public CameraManager cameraManager; // Reference to the CameraManager script

    // Voice lines logic
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for each voice line, must match length of voiceLines array

    private void Update()
    {
        if (!isActive)
        {
            activationTimer += Time.deltaTime;
            if (activationTimer >= activationDelay)
                isActive = true; // Activate Amelia after the delay
            else
                return; // Do not proceed with decay if not active
        }

        meterValue -= meterDecayRate * Time.deltaTime; // Decrease the meter value over time
        meterValue = Mathf.Clamp(meterValue, 0, meterMax); // Clamp the meter value between 0 and max
        UpdateMood();

        // Once they gave the modeler the materials, variants and animations, we can implement the mood changes based on meterValue.
        if (meterValue > meterMax * 0.5f)
            animator.Play("IdleHappy");
        else if(meterValue > meterMax * 0.2f)
            animator.Play("IdleWarning");
        else
            animator.Play("IdleDanger");
        if(meterValue <= 0)
        {
            DisableAllCameras();
        }
    }
    void UpdateMood()
    {
        if(meterValue > meterMax * 0.5f)
            meshRenderer.material = moodMaterials[0]; // Happy
        else if (meterValue > meterMax * 0.2f)
            meshRenderer.material = moodMaterials[1]; // Warning
        else
            meshRenderer.material = moodMaterials[2]; // Danger
    }
    void DisableAllCameras()
    {
        if (cameraManager)
        {
            cameraManager.ForceExitAndLockCameras(); // Force exit and lock cameras
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
