using Unity.VisualScripting;
using UnityEngine;

public class Scourage : MonoBehaviour
{
    public Transform doorTransform; // Reference to the door's transform
    public Transform originTransform; // Where they came from 
    public float moveSpeed = 5f; // Speed at which Scourage moves
    public bool charging = false; // Whether Scourage is currently charging
    public bool returning = false; // Whether Scourage is returning to origin

    // Voice lines logic 
    public AudioClip[] voiceLines;
    public float[] voiceLineWeights; // Weights for each voice line, must match length of voiceLines array
    private void Update()
    {
        if (charging)
        {
            MoveTo(doorTransform);
            if(Vector3.Distance(transform.position, doorTransform.position) < 0.5f)
            {
                charging = false;
                returning = true;

                // Triggering attack/power drain 
            }
        }
        else if(returning)
        {
            MoveTo(originTransform);
            if (Vector3.Distance(transform.position, originTransform.position) < 0.5f)
            {
                returning = false;
                // Reached origin, stop moving
                StartCharge();
            }
        }
    }
    void MoveTo(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        Vector3 lookDir = (target.position - transform.position).normalized;
        if(lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }
    public void StartCharge()
    {
        charging = true; 
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
