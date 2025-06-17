using System.Collections;
using UnityEngine;

public class CameraSaboteur : AnimatronicAI
{
    public CameraManager camManager; 
    public int sabotageWaypointIndex = 0; // Index of the waypoint to sabotage cameras at

    //public float sabotageCheckInterval = 10f; // How often to check in seconds, or how often to roll the dice 
    //[Range(0f, 1f)]
    //public float sabotageChance = 0.1f; // 0.1f = 10% chance, this goes to 1f to become 50% 

    //public CameraManager camManager;
    //private bool isSabotaging = false; 

    //private void Start()
    //{ 
    //    StartCoroutine(SabotageRoutine()); 
    //}
    //private IEnumerator SabotageRoutine()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(sabotageCheckInterval);

    //        if(!isSabotaging && Random.value <= sabotageChance)
    //        {
    //            StartCoroutine(SabotageCameras()); 
    //        }
    //    }
    //}
    //private IEnumerator SabotageCameras()
    //{
    //    isSabotaging = true; 

    //    yield return new WaitForSeconds(1f);

    //    Camera[] cams = camManager.cameras; 

    //    int index = Random.Range(1, cams.Length); // Skip index 0 

    //    camManager.DisableCamera(index);

    //    Debug.Log("Cameras sabotaged!");

    //    isSabotaging = false; 
    //}
    protected override IEnumerator ActOnArrival()
    {
        if(currentWaypointIndex == sabotageWaypointIndex)
        {
            // Diable all cameras except index 0 (security camera) 
            for(int i = 1; i < camManager.cameras.Length; i++)
            {
                camManager.DisableCamera(i);

                Debug.Log("All cameras sabotaged!"); 
                // Play a sound effect when this happens. 
            }
        }
        else
        {
            yield return base.ActOnArrival(); // Call the base method to handle normal behavior 
        }
    }
}
