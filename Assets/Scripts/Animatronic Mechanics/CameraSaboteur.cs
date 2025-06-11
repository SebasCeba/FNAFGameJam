using System.Collections;
using UnityEngine;

public class CameraSaboteur : MonoBehaviour
{
    public float sabotageCheckInterval = 10f; // How often to check in seconds, or how often to roll the dice 
    [Range(0f, 1f)]
    public float sabotageChance = 0.1f; // 0.1f = 10% chance, this goes to 1f to become 50% 

    public CameraManager camManager;
    private bool isSabotaging = false; 

    private void Start()
    { 
        StartCoroutine(SabotageRoutine()); 
    }
    private IEnumerator SabotageRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(sabotageCheckInterval);

            if(!isSabotaging && Random.value <= sabotageChance)
            {
                StartCoroutine(SabotageCameras()); 
            }
        }
    }
    private IEnumerator SabotageCameras()
    {
        isSabotaging = true; 

        yield return new WaitForSeconds(1f);

        Camera[] cams = camManager.cameras; 

        int index = Random.Range(1, cams.Length); // Skip index 0 

        camManager.DisableCamera(index);

        Debug.Log("Cameras sabotaged!");

        isSabotaging = false; 
    }
}
