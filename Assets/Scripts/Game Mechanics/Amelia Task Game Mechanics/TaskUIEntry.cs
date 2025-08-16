using TMPro;
using UnityEngine;

public class TaskUIEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text descriptionText; // Reference to the text component for the task description
    private string baseFormat;

    public void Setup(string TaskName, int total, float timeLimit, string uiFormat)
    {
        baseFormat = $"{TaskName} : {uiFormat}";
        UpdateProgress(0, total, timeLimit);
    }
    public void UpdateProgress(int current, int total, float timeLimit)
    {
        string text = string.Format(baseFormat, total, Mathf.CeilToInt(timeLimit));
        if (total > 1)
        {
            descriptionText.text = $"{text} ({current}/{total})";
        }
        else
        {
            descriptionText.text = text;
        }
    }
}
