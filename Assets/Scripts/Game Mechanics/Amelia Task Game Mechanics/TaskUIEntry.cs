using TMPro;
using UnityEngine;

public class TaskUIEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text descriptionText; // Reference to the text component for the task description
    private Task linkedTask; // The task this UI entry is linked to

    public void Initialize(Task task)
    {
        linkedTask = task; // Link the task to this UI entry
        UpdateText(); // Update the UI with the task details
    }
    private void Update()
    {
        if(linkedTask != null && !linkedTask.IsCompleted)
        {
            UpdateText(); // Continuously update the text to reflect any changes in the task
        }
    }
    private void UpdateText()
    {
        if (linkedTask != null)
        {
            descriptionText.text = $"{linkedTask.Description} - {Mathf.CeilToInt(linkedTask.TimeRemaining)}s"; // Update the text with the task description and time remaining
        }
    }
}
