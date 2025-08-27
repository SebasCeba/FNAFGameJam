using Artemis;
using UnityEngine;

public class Alera : MonoBehaviour
{
    public Player player;
    public int cameraOpenCount = 0;
    public int cameraRebootCount = 0;
    public float cameraTimeSpent = 0f;
    public float cameraTimeThreshold = 60f;
    public int openThreshold = 0;
    public int rebootThreshold = 3;
    public bool inOffice = false;

    private void Update()
    {
        // cameraOpenCount++; 
        // cameraRebootCount++;
        // cameraTimeSpent += Time.deltaTime; 

        if(!inOffice && (
            cameraTimeSpent > cameraTimeThreshold || 
            cameraTimeSpent > openThreshold ||
            cameraRebootCount > rebootThreshold))
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
