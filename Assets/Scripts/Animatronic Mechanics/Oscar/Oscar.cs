using UnityEngine;

public class Oscar : MonoBehaviour
{
    public Transform[] hideSpots;
    public float teleportInterval = 30f;
    private float timer; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TeleportToRandomSpot();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= teleportInterval)
        {
            TeleportToRandomSpot();
            timer = 0f;
        }
    }
    void TeleportToRandomSpot()
    {
        int idx = Random.Range(0, hideSpots.Length);
        transform.position = hideSpots[idx].position;

        // Face a direction 
        Vector3 lookDir = (Vector3.zero - transform.position).normalized;
        if(lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
