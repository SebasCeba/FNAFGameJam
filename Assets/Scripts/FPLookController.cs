using Artemis;
using Unity.Cinemachine;
using UnityEngine;

public class FPLookController : MonoBehaviour
{
    [Header("Components")]
    private FPController controller;
    [SerializeField] CinemachineCamera fpCamera;

    [Header("Look Parameters")]
    public float Pitchlimit = 85f;
    [SerializeField] float currentPitch = 0f;

    public float yawLimit = 45f; //max left and right angle from starting yaw 
    private float currentYaw = 0f;
    private float startingYaw;

    public bool canLook = true; 
    public float CurrentPitch
    {
        get => currentPitch;

        set
        {
            currentPitch = Mathf.Clamp(value, -Pitchlimit, Pitchlimit);
        }
    }

    [Header("Inputs")]
    public Vector2 LookInput;
    public Vector2 LookSensitivity = new Vector3(0.1f, 0.1f);
    #region Unity Methods 
    private void Start()
    {
        startingYaw = transform.eulerAngles.y; 
    }
    private void Update()
    {
        LookUpdate();
    }
    #endregion
    #region Look methods 
    void LookUpdate()
    {
        if(!canLook) return; 

        Vector2 input = new Vector2(LookInput.x * LookSensitivity.x, LookInput.y * LookSensitivity.y);

        // Vertical Look (pitch)
        CurrentPitch -= input.y;

        fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

        // Horizontal look (yaw) - clamped 
        currentYaw += input.x; 
        currentYaw = Mathf.Clamp(currentYaw, - yawLimit, yawLimit);
        float clampedYaw = startingYaw + currentYaw; 

        // Lokking left and right 
        //transform.Rotate(Vector3.up * input.x);
        transform.rotation = Quaternion.Euler(0f, clampedYaw, 0f);
    }
    #endregion 
}
