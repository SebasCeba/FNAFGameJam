using TMPro;
using UnityEngine;

public class PowerSystem : MonoBehaviour
{
    public int SystemsOn;
    public float Power = 100; // Total power available
    [SerializeField] private TextMeshProUGUI powerPercentageText;

    private void Update()
    {
        float powerDrain = 0f;

        switch (SystemsOn)
        {
            case 1: 
                powerDrain = 0.1f; // Drain for one system
                break;
            case 2:
                powerDrain = 1f; // Drain for two systems
                break;
            case 3:
                powerDrain = 1.5f; // Drain for three systems
                break;
            case 4:
                powerDrain = 2f; // Drain for four systems
                break;
            case 5:
                powerDrain = 3f; // Drain for five systems
                break;
        }

        Power -= powerDrain * Time.deltaTime; // Drain power over time
        Power = Mathf.Clamp(Power, 0f, 100); // Ensure power does not go below 0 or above 100

        // Update the power percentage text
        string powerText = string.Format("{0:0}", Power);
        powerPercentageText.text = $"{powerText}%"; // Update the power percentage text
    }
}
