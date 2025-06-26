using TMPro;
using UnityEngine;

public class ShiftTimer : MonoBehaviour
{
    [SerializeField] private float Timer;
    [SerializeField] private int ShiftEndTime = 6;
    [SerializeField] private string DigitalClock;

    [SerializeField] private float TimeMultiplier = 2f; // Multiplier for the timer speed

    [SerializeField] private TextMeshProUGUI clockText;

    [SerializeField] private GameObject WinScreen; // Reference to the win screen GameObject
    private bool isWon; // Flag to check if the game has been won
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DigitalClock = ""; // Initialize the digital clock display
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWon)
        {
            Timer += Time.deltaTime * TimeMultiplier; // Increment the timer by delta time multiplied by the speed multiplier

            var hour = Mathf.FloorToInt(Timer / 60);
            var minute = Mathf.FloorToInt(Timer - hour * 60);

            if (hour >= 6)
            {
                WinScreen.SetActive(true); // Activate the win screen when the shift ends
                isWon = true; // Set the flag to indicate the game has been won
            }
            if (hour == 0)
            {
                hour = 12; // Convert 0 hours to 12 for display
            }

            DigitalClock = string.Format("{0:00}:{1:00}", hour, minute); // Format the digital clock display

            clockText.text = DigitalClock;
        }
    }
}
