using Artemis;
using UnityEngine;

public class Alera : MonoBehaviour
{
    public Player player;
    public JumpscareManager jsManager; // Reference to jumpscare manager
    public Transform aleraRoomPosition; // Position in the room where alera appears
    public Transform officePosition; // Position in the office where alera appears

    public float activationDelay = 30f;
    private bool isActive = false; 
    private float activationTimer = 0f;

    private int cameraOpenCount = 0;
    public int cameraOpenThreshold; // Number of times cameras must be opened to trigger Alera
    private bool lastCameraStateInOffice = false; // Track last camera state in office
    private bool lastCameraState = false; // Track last camera state
    public bool inOffice = false;

    public float officeStayDuration = 5f; // Time Alera stays in the office before attacking
    private float officeTimer = 0f;

    // Voice lines logic
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for each voice line, must match length of voiceLines array

    private void Update()
    {
        if(!isActive)
        {
            activationTimer += Time.deltaTime;
            if(activationTimer >= activationDelay)
            {
                isActive = true;
            }
            else
                return; // Skip the rest of the update until Alera is active
        }

        // Detect camera open event (transition from closed to open 
        bool camerasOpen = player != null && player.camManager != null && player.camManager.CamerasOpen;
        if(camerasOpen && !lastCameraState && !inOffice)
        {
            cameraOpenCount++;

            // Trigger events at certain thresholds 
            if(cameraOpenCount == 1)
            {
                // Trigger firest event (poster change)
                
            }
            else if(cameraOpenCount == 4)
            {
                // Trigger second event (Lights flicker)

                AppearInOffice();
            }
            // Add more thresholds/events as needed 

            //When threshold reached, alera appears in the office
            //if (cameraOpenCount >= cameraOpenThreshold)
            //{
            //    AppearInOffice();
            //}
        }
        lastCameraState = camerasOpen;

        // Handle Alera's behavior in the office
        if(inOffice)
        {
            officeTimer += Time.deltaTime;

            // Shoo alera away if cameras are opened again
            if(camerasOpen && !lastCameraStateInOffice)
            {
                ResetAlera();
                return; // Exit early to avoid triggering jumpscare
            }
            lastCameraStateInOffice = camerasOpen;

            // If jumpscare if timer runs out 
            if (officeTimer >= officeStayDuration)
            {
                if(jsManager != null)
                {
                    jsManager.TriggerJumpscare(AnimatronicType.Alera);
                }
                else
                {
                    Debug.LogWarning("JumpscareManager reference not set on Alera.");
                }
                ResetAlera();
            }
        }
        else
        {
            lastCameraStateInOffice = false; // Reset when not in office
        }
    }
    void AppearInOffice()
    {
        inOffice = true;
        officeTimer = 0f; // Reset timer
        lastCameraStateInOffice = player != null && player.camManager != null && player.camManager.CamerasOpen;
        // Teleport Alera to the office position
        if (officePosition != null)
        {
            transform.position = officePosition.position;
        }
    }
    void ResetAlera()
    {
        inOffice = false;
        officeTimer = 0f;
        cameraOpenCount = 0;
        lastCameraState = false;
        lastCameraStateInOffice = false;
        // Teleport Alera back to her room if needed
        if (aleraRoomPosition != null)
        {
            transform.position = aleraRoomPosition.position;
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
