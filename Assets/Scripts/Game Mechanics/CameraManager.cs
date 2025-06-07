using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera camera_securityRoom; // main camera 
    public Camera camera_1A;
    public Camera camera_1B;
    public Camera camera_1C;
    public Camera camera_2A;
    public Camera camera_2B;
    public Camera camera_2C;
    public Camera camera_3;
    public Camera camera_4A;
    public Camera camera_4B;

    protected Camera[] cameras;
    protected int currentCamera;

    //[SerializeField] private bool CamerasOpen;
    [SerializeField] private GameObject camUI;

    public bool CamerasOpen { get; private set; } = false;
    private void Awake()
    {
        cameras = new Camera[10];
        
        cameras[0] = camera_securityRoom;
        cameras[1] = camera_1A;
        cameras[2] = camera_1B;
        cameras[3] = camera_1C;
        cameras[4] = camera_2A;
        cameras[5] = camera_2B;
        cameras[6] = camera_2C;
        cameras[7] = camera_3;
        cameras[8] = camera_4A;
        cameras[9] = camera_4B;
    }
    public void IncreaseCamera()
    {
        cameras[currentCamera].GetComponent<AudioListener>().enabled = false;
        cameras[currentCamera].enabled = false; 

        currentCamera++;

        if(currentCamera >= cameras.Length)
        {
            currentCamera = 0;
        }
        cameras[currentCamera].enabled = true;
        cameras[currentCamera].GetComponent<AudioListener>().enabled = true;
    }
    public void DecreaseCamera()
    {
        cameras[currentCamera].GetComponent<AudioListener>().enabled = false;
        cameras[currentCamera].enabled = false; 

        currentCamera--;

        if (currentCamera < 0)
        {
            currentCamera = cameras.Length - 1;
        }
        cameras[currentCamera].enabled = true;
        cameras[currentCamera].GetComponent<AudioListener>().enabled = true;
    }
    public void OpenCam()
    {
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
        //if(CamerasOpen)
        //{
        //    camUI.SetActive(true); 
        //}
        //else
        //{
        //    camUI.SetActive(false);
        //}
    }
    public void SwitchCams(int index)
    {
        if (index < 0 || index >= cameras.Length) return; 

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

        currentCamera = index;
    }
}
