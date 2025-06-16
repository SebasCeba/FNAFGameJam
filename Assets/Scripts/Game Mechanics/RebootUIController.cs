using Unity.UI.Shaders.Sample;
using UnityEngine;
using UnityEngine.UI;

public class RebootUIController : MonoBehaviour
{
    [SerializeField] private Button rebootButton;
    [SerializeField] private float rebootCooldown = 10f;
    [SerializeField] private CameraManager camManager;

    [Header("Meter Reference")]
    [SerializeField] private Meter rebootMeter;

    [Header("Meter Visual Cycle")]
    [SerializeField] private float meterCycleDuration = 5f; // Reset every 5 seconds 
    private float meterCycleTimer = 0f;

    private float lastRebootTime = -Mathf.Infinity;

    private void Start()
    {
        rebootButton.onClick.AddListener(TryReboot);
        rebootMeter.Value = 0f; // Initialize the meter value
    }
    private void Update()
    {
        // Meter: Loop every 5 seconds 
        meterCycleTimer += Time.deltaTime;
        float meterProgress = Mathf.Clamp01(meterCycleTimer / meterCycleDuration);
        rebootMeter.Value = 1f - meterProgress; // 1 = empty color 

        if(meterCycleTimer >= meterCycleDuration)
        {
            meterCycleTimer = 0f; // Reset the timer
        }

        // Button: Cooldown logic 
        float timeSinceLastReboot = Time.time - lastRebootTime;
        rebootButton.interactable = timeSinceLastReboot >= rebootCooldown;
    }
    private void TryReboot()
    {
        if(Time.time - lastRebootTime < rebootCooldown)
        {
            Debug.Log("Reboot is on cooldown.");
            return;
        }

        lastRebootTime = Time.time;
        camManager.TryRebootCamera();

        Debug.Log("Rebooting camera...");
    }
}
