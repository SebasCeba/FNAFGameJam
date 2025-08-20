using System.Collections.Generic;
using UnityEngine;
using System;

public class TaskInstance
{
    public TaskData Data { get; private set; }
    public float TimeRemaining; // Time remaining to complete the task, if applicable
    public bool IsCompleted;
    public Action OnComplete;
    public Action OnFail; // Optional action to perform on failure

    public TaskType taskType;
    public List<GameObject> SpawnedObjects { get; private set; }
    public int CompletedCount = 0;
    public int TotalObjects => SpawnedObjects.Count;
    //public float TimeRemaining => Data.timeLimit; // Assuming time limit is constant for the task
    public bool IsComplete => CompletedCount >= TotalObjects;

    public TaskUIEntry UIEntry { get; set; }

    public TaskInstance(TaskData data, Action onComplete = null, Action onFail = null)
    {
        Data = data;
        TimeRemaining = data.timeLimit; // Set the time limit from the task data
        IsCompleted = false;
        OnComplete = onComplete;
        OnFail = onFail;
        //CompletedCount = 0;
    }

    public bool MarkObjectCompleted(TaskInteractable obj)
    {
        if (SpawnedObjects.Contains(obj.gameObject))
        {
            CompletedCount++;
            if (UIEntry != null)
                UIEntry.UpdateProgress(CompletedCount, TotalObjects, TimeRemaining);
            return true;
        }
        return false;
    }
    public void UpdateTask(float deltaTime)
    {
        if (!IsCompleted)
        {
            TimeRemaining -= deltaTime;
            if (TimeRemaining <= 0f)
            {
                Fail();
            }
        }
    }
    public void Complete()
    {
        if (IsCompleted)
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
