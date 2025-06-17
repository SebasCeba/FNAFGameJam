using UnityEngine;

public class StalkerAnimatronic : AnimatronicAI
{
    public float viewAngleThreshold = 30f;
    public float slowedSpeed = 1.0f;
    public float normalSpeed = 3.5f;

    protected override void Start()
    {
        base.Start();
        agent.speed = normalSpeed; // Set initial speed to normal
    }
    protected override void Update()
    {
        float closestDistance = float.MaxValue;
        Camera lookingCamera = null;

        // Find the closest camera and the minimum angle to it
        foreach(var cam in cameras)
        {
            if (!cam.enabled || !cam.gameObject.activeInHierarchy)
                continue; // Skip cameras that are not enabled or active

            Vector3 directionToAnimatronic = (transform.position - cam.transform.position).normalized;
            float angle = Vector3.Angle(cam.transform.forward, directionToAnimatronic);

            if (angle < viewAngleThreshold)
            {
                float distance = Vector3.Distance(transform.position, cam.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    lookingCamera = cam; // Update the closest camera
                }
            }
        }
        if(lookingCamera != null)
        {
            agent.isStopped = true; // Stop the agent if a camera is looking at the animatronic
            Debug.Log($"[StalkerAnimatronic] Closest camera: '{lookingCamera.name}', is LOOKING at animatronic! Stop moving!");
        }
        else if(IsAnyCameraLookingAtMe())
        {
            agent.isStopped = false; // Resume movement if no cameras are looking
            agent.speed = slowedSpeed; // Slow down if the player is looking at the animatronic
        }
        else
        {
            agent.isStopped = false; // Resume movement if no cameras are looking
            agent.speed = normalSpeed; // Resume normal speed if no cameras are looking
        }
        base.Update(); // Call the base Update method to continue normal behavior
    }
    private bool IsAnyCameraLookingAtMe()
    {
        if(cameras == null || cameras.Length == 0)
        {
            return false; // No cameras to check
        }
        foreach(var cam in cameras)
        {
            if(!cam.enabled || !cam.gameObject.activeInHierarchy)
            {
                continue; // Skip cameras that are not enabled or active
            }
            Vector3 directionToAnimatronic = (transform.position - cam.transform.position).normalized;

            float angle = Vector3.Angle(cam.transform.forward, directionToAnimatronic);

            Debug.Log($"[StalkerAnimatronic] Camera: '{cam.name}', Angle to animatronic: {angle:F2} (theshold: {viewAngleThreshold})");

            if (angle < viewAngleThreshold)
                Debug.Log($"[StalkerAnimatronic] Camera '{cam.name}' is LOOKInG at the animatronic!");
            return true; // Return true if the angle is within the threshold
        }
        return false;
    }
}
