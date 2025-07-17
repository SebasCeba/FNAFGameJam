using UnityEngine;

public class Task
{
    public string Description;
    public float TimeRemaining = 30f; // Time remaining to complete the task, if applicable
    public bool IsCompleted;
    public System.Action OnComplete;

    public Task(string description, System.Action onComplete = null)
    {
        Description = description;
        IsCompleted = false;
        OnComplete = onComplete;
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
        IsCompleted = true;
        OnComplete?.Invoke();
    }
    private void Fail()
    {
        Debug.Log("Task failed!");
        // Notify manager or animatronic to disable the camera...DOOOM
    }
}
