using UnityEngine;

public class Task
{
    public string Description;
    public float TimeRemaining = 30f; // Time remaining to complete the task, if applicable
    public bool IsCompleted;

    public TaskType Type;
    public System.Action OnComplete;
    public System.Action OnFail; // Optional action to perform on failure

    public Task(string description, TaskType type, System.Action onComplete = null, System.Action onFail = null)
    {
        Description = description;
        Type = type; 
        IsCompleted = false;
        OnComplete = onComplete;
        OnFail = onFail;
    }
    public void UpdateTask(float deltaTime)
    {
        if (!IsCompleted)
        {
            TimeRemaining -= deltaTime;
            if(TimeRemaining <= 0f)
            {
                Fail();
            }
        }
    }
    public void Complete()
    {
        if(IsCompleted)
        {
            Debug.LogWarning("Task is already completed!");
            return;
        }
        IsCompleted = true;
        OnComplete?.Invoke();
    }
    private void Fail()
    {
        Debug.Log("Task failed!");
        OnFail?.Invoke();
        // Notify manager or animatronic to disable the camera...DOOOM
    }
}

public enum TaskType
{
    CleanOffice,
    SendSupplies,
    KeepDoorOpen,
    KeepLightsOn,
}

