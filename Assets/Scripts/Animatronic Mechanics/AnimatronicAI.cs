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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextWayPoint();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            ActOnArrival();
        }
    }
    protected virtual IEnumerator ActOnArrival()
    {
        isWaiting = true; // Set waiting state

        // 1. Look at closest camera 
        Camera closetCam = FindClosestCamera(); 
        if(closetCam != null)
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
}
