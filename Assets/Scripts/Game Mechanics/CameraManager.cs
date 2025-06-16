using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Array")]
    public Camera[] cameras;
    protected int currentCamera;

    [Header("Camera Tracking")]
    //[SerializeField] private bool CamerasOpen;
    private HashSet<int> disabledCameraIndices = new HashSet<int>(); // Tracks disabled camera 
    [Header("Camera Interactions")]
    [SerializeField] private float rebootCooldown = 10f;
    [SerializeField] private AudioClip switchSound;
    [SerializeField] private AudioClip denySound;
    [SerializeField] private AudioSource audioSource; 

    private float lastRebootTime = -Mathf.Infinity;

    [Header("References")]
    [SerializeField] private GameObject camUI;
    public bool CamerasOpen { get; private set; } = false;
    private bool canSwitchCameras = true; // Can we switch cameras?
    private void Awake()
    {
        if(cameras == null || cameras.Length == 0)
        {
            Debug.LogWarning("No cameras assigned in cameraManager"); 
        }
    }
    public void DisableCamera(int index)
    {
        if(index > 0 && index < cameras.Length && cameras[index] != null)
        {
            cameras[index].enabled = false;
            if (cameras[index].TryGetComponent<AudioListener>(out var listner))
                listner.enabled = false;

            disabledCameraIndices.Add(index); // track it 
            Debug.Log($"Camera {index} is disabled.");
        }
    }
    public void OpenCam()
    {
        if(!CamerasOpen && AllCamerasOffline())
        {
            Debug.LogWarning("All cameras are sabotaged! Reboot required");
            PlayDenySound();
            canSwitchCameras = false; // Prevent switching cameras if all are offline 
            return; 
        }
        CamerasOpen = !CamerasOpen; 
        ShowCamera();
    }
    public void ShowCamera()
    {
        camUI.SetActive(CamerasOpen);
        if(!CamerasOpen)
        {
            // Return to the main camera 
            SwitchCams(0);
        }
    }
    public void SwitchCams(int index)
    {
        if (!canSwitchCameras) return; // Prevent switching if not allowed

        if (index < 0 || index >= cameras.Length) return; 

        // Blocks if sabotaged 
        if(disabledCameraIndices.Contains(index))
        {
            Debug.LogWarning($"Camera {index} is sabotaged. Cannot view.");
            audioSource?.PlayOneShot(denySound); // Play the deny sound 
            return; 
        }

        // Disable all cameras 
        foreach(var cam in cameras)
        {
            cam.enabled = false;
            if (cam.TryGetComponent<AudioListener>(out var listener))
                listener.enabled = false; 
        }

        // Enable the chosen one 
        cameras[index].enabled = true;
        if (cameras[index].TryGetComponent<AudioListener>(out var activeListnerer))
            activeListnerer.enabled = true;

        audioSource?.PlayOneShot(switchSound); 
        currentCamera = index;
    }
    public void TryRebootCamera()
    {
        if(Time.time - lastRebootTime < rebootCooldown)
        {
            Debug.Log("reboot on cooldown");
            return; 
        }
        foreach(int index in disabledCameraIndices)
        {
            cameras[index].enabled = true;
            if (cameras[index].TryGetComponent<AudioListener>(out var listener))
                listener.enabled = false; 
        }
        disabledCameraIndices.Clear();
        lastRebootTime = Time.time;
        canSwitchCameras = true; // Allow switching cameras again after reboot
        Debug.Log("Cameras rebooted."); 
    }
    private bool AllCamerasOffline()
    {
        // skip index 0 if it's the secuirty room camera 
        for(int i = 1; i < cameras.Length; i++)
        {
            if (!disabledCameraIndices.Contains(1))
            {
                return false; // At least one camera is working 
            }
             
        }
        return true; // All non-security room cameras are disabled
    }
    private void PlayDenySound()
    {
        audioSource.PlayOneShot(denySound);
    }
}
