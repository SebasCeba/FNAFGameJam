using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AnimatronicAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTimeAtPoint = 2f;
    public float lookAtCameraTime = 2;
    public Camera[] cameras; // Assign from Camera Manager or Inspector 

    protected NavMeshAgent agent;
    protected int currentWaypointIndex = -1;
    protected bool isWaiting = false;

    public Door officeDoor; // Assign in Inspector if needed
    public int officeWaypointIndex = -1;
    public float waitAtOfficeTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextWayPoint();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            StartCoroutine(ActOnArrival());
        }
    }
    protected virtual IEnumerator ActOnArrival()
    {
        isWaiting = true; // Set waiting state

        // Check if at the office {
        if(currentWaypointIndex == officeWaypointIndex)
        {
            Debug.Log($"{name} reached the office door.");

            // Check door state 
            if(officeDoor != null)
            {
                // Door is closed 
                Debug.Log($"{name}: Door is CLOSED, waiting {waitAtOfficeTime}s...");

                yield return new WaitForSeconds(waitAtOfficeTime); // Wait at the office door

                if(officeDoor.IsOpen)
                {
                    Debug.Log($"{name}: Door is OPEN, entering office...");
                    EnterOffice(); // Enter the office if the door is open
                    yield break; // End coroutine after entering office
                }
            }
            else
            {
                // Door is open 
                Debug.Log($"{name}: Door was closed, going to next waypoint..."); // Game over, jump scare, etc

                GoToNextWayPoint(); // Go to the next waypoint after waiting
            }
        }
        else
        {
            // 1. Look at closest camera 
            Camera closetCam = FindClosestCamera();
            if (closetCam != null)
            {
                Vector3 lookPos = closetCam.transform.position - transform.position;
                lookPos.y = 0; // Keep upright 
                Quaternion lookRot = Quaternion.LookRotation(lookPos);
                float t = 0;
                while (t < lookAtCameraTime)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
                    t += Time.deltaTime;
                    yield return null; // Wait for next frame 
                }
            }
            // 2. Wait at the waypoint 
            yield return new WaitForSeconds(waitTimeAtPoint);

            // 3. Move to the next random waypoint (avoiding grouping)
            GoToNextWayPoint();
        }

        isWaiting = false; // Reset waiting state
    }
    protected void GoToNextWayPoint()
    {
        int nextIndex = currentWaypointIndex;
        int attempts = 0;
        do
        {
            nextIndex = Random.Range(0, waypoints.Length);
            attempts++;
        }
        // Avoid current point and points occupid by other animatronics 
        while((nextIndex == currentWaypointIndex || IsWaypointOccupied(nextIndex)) && attempts < 10);

        currentWaypointIndex = nextIndex; 
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }
    protected bool IsWaypointOccupied(int waypointIndex)
    {
        // Find all animatronics in the scne 
        var animatronics = FindObjectsByType<AnimatronicAI>(FindObjectsSortMode.InstanceID); 
        return animatronics.Any(a => a != this && a.currentWaypointIndex == waypointIndex);
    }
    protected Camera FindClosestCamera()
    {
        if(cameras == null || cameras.Length == 0)
        {
            Debug.LogWarning("No cameras assigned to AnimatronicAI");
            return null;
        }
        return cameras.OrderBy(cam => Vector3.Distance(transform.position, cam.transform.position))
                      .FirstOrDefault();
    }
    protected virtual void EnterOffice()
    {
        Debug.Log("${name} entered the office! Ha You got tickled");
        // Should play a jumpscare, freeze the inputs of the player or look at the door that has the inturder coming in. 
    }
}
