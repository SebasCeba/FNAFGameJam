using System.Collections.Generic;
using UnityEngine;

public class TaskInstance
{
    public TaskData Data { get; private set; }
    public TaskType taskType;
    public List<GameObject> SpawnedObjects { get; private set; }
    public int CompletedCount { get; private set; }
    public int TotalObjects => SpawnedObjects.Count;
    public float TimeRemaining => Data.timeLimit; // Assuming time limit is constant for the task
    public bool IsComplete => CompletedCount >= TotalObjects;

    public TaskUIEntry UIEntry { get; set; }

    public TaskInstance(TaskData data, List<GameObject> objects)
    {
        Data = data;
        SpawnedObjects = objects;
        CompletedCount = 0;
    }

    public bool MarkObjectCompleted(TaskOB obj)
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
}
