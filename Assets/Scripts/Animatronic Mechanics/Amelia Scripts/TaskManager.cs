using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public TaskUIManager taskUIManager; // Reference to the UI manager for tasks
    public List<TaskInstance> ActiveTasks = new List<TaskInstance>();
    //public System.Action OnAnyTaskFailed; 
    //private void Update()
    //{
    //    for(int i = ActiveTasks.Count -1; i >= 0; i--)
    //    {
    //        var task = ActiveTasks[i];
    //        float oldTime = task.TimeRemaining; 
    //        task.UpdateTask(Time.deltaTime);

    //        if(!task.IsCompleted && oldTime > 0 && task.TimeRemaining <= 0)
    //        {
    //            OnAnyTaskFailed?.Invoke();
    //            ActiveTasks.RemoveAt(i);
    //            taskUIManager?.RemoveTaskUI(task); // Update UI when a task fails
    //        }
    //    }
    //}
    public void CreateTask(TaskData data, QuestGiverAnimatronic giver)
    {
        Debug.Log($"[TaskManager] Creating task from data: {data.taskName}");

        // Spawn objects if needed 
        List<GameObject> spawnedObjects = new List<GameObject>();
        TaskInstance instance = new TaskInstance(data, spawnedObjects); // Create a new task instance with the data and empty list of spawned objects

        if (data.objectPrefab != null && data.spawnPoints.Length > 0)
        {
            for(int i = 0; i < data.spawnPoints.Length; i++)
            {
                Transform spawnPoint = data.spawnPoints[Random.Range(0, data.spawnPoints.Length)];
                GameObject obj = Instantiate(data.objectPrefab, spawnPoint.position, spawnPoint.rotation);
                TaskOB taskObj = obj.GetComponent<TaskOB>();
                if(taskObj != null)
                {
                    taskObj.Setup(instance); 
                    spawnedObjects.Add(obj);
                }
            }
        }
        ActiveTasks.Add(instance);

        // Create UI through TaskUIManager
        taskUIManager.AddTaskUI(instance, giver.GetTaskUIPrefab());
    }
    public void ReportObjectCompleted(TaskOB task)
    {
        TaskInstance instance = task.LinkedInstance; // Get the linked task instance
        if (instance == null) return; // If no linked instance, do nothing

        if (task.IsCompleted)
        {
            CompleteTask(instance);
        }
    }
    //public void AddTask(Task task)
    //{
    //    Debug.Log($"Adding task: {task.Description}");
    //    ActiveTasks.Add(task);
    //    taskUIManager?.AddTaskUI(task);
    //}
    public void CompleteTask(TaskInstance task)
    {
        //task.Complete();
        // Will remove the task from the list and update UI 
        ActiveTasks.Remove(task);
        taskUIManager?.RemoveTaskUI(task);
    }
}
