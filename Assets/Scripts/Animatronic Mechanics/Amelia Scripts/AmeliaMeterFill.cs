using Unity.UI.Shaders.Sample;
using UnityEngine;
using UnityEngine.InputSystem;

public class AmeliaMeterFill : MonoBehaviour
{
    public Meter meterSlider; // Reference to the UI Slider for the meter
    public Amelia amelia; // Reference to the Amelia script
    public float fillRate; // Rate at which the meter fills when the mouse button is held down
    public void Update()
    {
        // If the left mouse button is held down 
        if(Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            IncreaseMeter(fillRate * Time.deltaTime); // Increase the meter by 20 units per second
        }
    }
    public void IncreaseMeter(float amount)
    {
        amelia.meterValue = Mathf.Clamp(amelia.meterValue + amount, 0, amelia.meterMax);
        amelia.UpdateMood();
    }
}
