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
}
