using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

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
    [SerializeField] private Transform laptopTransform; // Assign in Inspector if camUI is an UI element
    private Tween laptopTween; 
    public bool CamerasOpen { get; private set; } = false;
    private bool canSwitchCameras = true; // Can we switch cameras?
    private int lastCameraIndex = 0; // To track last camera before all were disabled
    [SerializeField] private int defaultCameraIndex = 1; // Set to front stage camera index in Inspector 
    private bool firstOpenThisNight = true; // To track if it's the first time opening cameras this night

    private Vector3 laptopClosedPos;
    private Vector3 laptopOpenPos;
    private Vector3 laptopClosedRot;
    private Vector3 laptopOpenRot;

    [SerializeField] public PowerSystem power;
    private void Awake()
    {
        if(cameras == null || cameras.Length == 0)
        {
            Debug.LogWarning("No cameras assigned in cameraManager"); 
        }
        if(laptopTransform != null)
        {
            laptopClosedPos = laptopTransform.localPosition;
            laptopClosedRot = laptopTransform.localEulerAngles;

            laptopOpenPos = laptopClosedPos + new Vector3(0f, 0f, 0.5f); // Adjust as needed
            laptopOpenRot = new Vector3(90f, laptopClosedRot.y, laptopClosedRot.z); // Adjust as needed
        }
    }
    private void Update()
    {
        if(power.Power <= 0f)
        {
            // Force cameras to close 
            if(CamerasOpen)
            {
                CamerasOpen = false;
                power.SystemsOn -= 1; // Decrement power system count when cameras are closed
                ShowCamera(); // Hide camera UI
            }
            camUI.SetActive(false); // Ensure camera UI is hidden when power is out
            canSwitchCameras = false; // Prevent switching cameras when power is out
        }
        else
        {
            // If power is restored and acameras are open, allow switching again
            if (CamerasOpen)
            {
                canSwitchCameras = true;
            }
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
        if (CamerasOpen)
        {
            power.SystemsOn += 1; // Increment power system count when cameras are opened
        }
        else
        {
            power.SystemsOn -= 1; // Decrement power system count when cameras are closed
        }
        ShowCamera();
    }
    public void ShowCamera()
    {
        //camUI.SetActive(CamerasOpen);
        AnimateLaptop(CamerasOpen);
        if (CamerasOpen)
        {
            if (firstOpenThisNight)
            {
                // Open last viewed camera
                SwitchCams(defaultCameraIndex);
                firstOpenThisNight = false; // No longer the first open this night
            }
            else
            {
                SwitchCams(lastCameraIndex); // Switch to last viewed camera
            }
            
        }
        else
        {
            SwitchCams(0, false); // Do not update last camera index when switching back to main cam
        }
    }
    public void ResetCameraNight()
    {
        firstOpenThisNight = true;
        lastCameraIndex = defaultCameraIndex; // Reset last camera index
    }
    public void AnimateLaptop(bool open)
    {
        if (laptopTransform == null) return; 
        if(laptopTransform != null && laptopTween.IsActive()) laptopTween.Kill();

        Vector3 targetPos = open ? laptopOpenPos : laptopClosedPos;
        Vector3 targetRot = open ? laptopOpenRot : laptopClosedRot;

        //camUI.SetActive(true); // Ensure it's active to see the animation
        // Rotate around the X axis from closed to open position
        //float targetAngle = open ? 90f : 0f; // Adjust angles as needed\

        // Animate position and rotation together, slower for visibility 
        laptopTween = laptopTransform.DOLocalMove(targetPos, 0.6f).SetEase(Ease.OutCubic);
        laptopTransform.DOLocalRotate(targetRot, 0.6f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                if(open)
                {
                    camUI.SetActive(true); // Activate UI after opening animation
                }
                else
                {
                    camUI.SetActive(false); // Deactivate UI after closing animation
                }
            });
    }
    public void SwitchCams(int index, bool updateLastCamera = true)
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
        if(updateLastCamera)
        {
            lastCameraIndex = index; // Update last camera index only if specified
        }
    }
    public void SwitchCamsUI(int index)
    {
        SwitchCams(index, true); // Always update last cameras when called from UI
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
            if (!disabledCameraIndices.Contains(i))
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
    public void ForceExitAndLockCameras()
    {
        CamerasOpen = true; 
        camUI.SetActive(false);
        canSwitchCameras = false; // Lock cameras from switching

        // Assume cameras[0] is the main player camera (security room) 
        for(int i = 1; i < cameras.Length; i++)
        {
            cameras[i].enabled = false;
            if (cameras[i].TryGetComponent<AudioListener>(out var listener))
                listener.enabled = false;
        }
        // Ensure main cameras stays enabled 
        if (cameras.Length > 0)
        {
            cameras[0].enabled = true;
            if (cameras[0].TryGetComponent<AudioListener>(out var listener))
                listener.enabled = true;
        }
    }
}
