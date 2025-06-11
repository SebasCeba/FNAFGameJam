using System.Collections;
using UnityEngine;

public class CameraSaboteur : AnimatronicAI
{
    public float sabotageDelay = 5f;
    private bool hasSabotaged = false;
    public CameraManager camManager;

    protected override void ActOnArrival()
    {
        base.ActOnArrival();

        if(!hasSabotaged && camManager != null)
        {
            StartCoroutine(SabotageCameras());
            hasSabotaged = true;
        }
        // If it's the fianl waypoint, start sabotage 
        if(currentWaypointIndex >= waypoints.Length && !hasSabotaged)
        {
            
        }
    }
    private IEnumerator SabotageCameras()
    {
        yield return new WaitForSeconds(sabotageDelay);

        Camera[] cams = camManager.cameras; 

        int randomIndex = Random.Range(1, cams.Length); // Skip index 0 

        camManager.DisableCamera(randomIndex);

        //  Example: Send event to camera manager 
        //camManager.DisableRandomCamera();

        Debug.Log("Cameras sabotaged!");
    }
}
