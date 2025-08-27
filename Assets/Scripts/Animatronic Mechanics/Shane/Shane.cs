using Artemis;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Shane : MonoBehaviour
{
    public Transform[] wanderPoints;
    public Transform windowPoint;
    public Transform doorPoint;
    public Transform playerPoint;
    public float moveSpeed = 3f;
    public float observeTime = 5f;
    private int currentPoint = 0;
    private bool observing = false;
    private float observeTimer = 0f;

    public FPController player; 
    private void Update()
    {
        if (observing)
        {
            observeTimer += Time.deltaTime; 
            transform.LookAt(playerPoint);
            if(observeTimer >= observeTime)
            {
                observing = false;
                // Player death jumpscare. 
                if(player != null)
                {
                    //player.Jumpscare("Shane"); // Call jumpscare logic 
                }
            }
        }
        else
        {
            Patrol(); 
        }
    }
    public void Patrol()
    {
        if(wanderPoints.Length == 0) return;
        Transform target = wanderPoints[currentPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentPoint = (currentPoint + 1) % wanderPoints.Length;
            if(target == windowPoint || target == doorPoint)
            {
                observing = true;
                observeTimer = 0f;
            }
        }
    }
}
