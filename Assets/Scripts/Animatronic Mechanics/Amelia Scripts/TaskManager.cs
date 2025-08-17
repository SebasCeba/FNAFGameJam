using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public TaskUIManager taskUIManager; // Reference to the UI manager for tasks
    public List<TaskInstance> ActiveTasks = new List<TaskInstance>();
    public System.Action OnAnyTaskFailed;
    private void Update()
    {
        for (int i = ActiveTasks.Count - 1; i >= 0; i--)
        {
            var task = ActiveTasks[i];
            float oldTime = task.TimeRemaining;
            task.UpdateTask(Time.deltaTime);

            if (!task.IsComplete && oldTime > 0 && task.TimeRemaining <= 0)
            {
                OnAnyTaskFailed?.Invoke();
                ActiveTasks.RemoveAt(i);
                taskUIManager?.RemoveTaskUI(task); // Update UI when a task fails
            }
        }
    }
    public void CreateTask(TaskData data, QuestGiverAnimatronic giver, System.Action onComplete = null, System.Action onFail = null)
    {
        Debug.Log($"[TaskManager] Creating task from data: {data.taskName}");

        TaskInstance instance = new TaskInstance(data, onComplete, onFail); // Create a new task instance with the data and empty list of spawned objects

        ActiveTasks.Add(instance);
        // Create UI through TaskUIManager
        taskUIManager.AddTaskUI(instance, giver.GetTaskUIPrefab());
    }
    public void ReportObjectCompleted(TaskInteractable task)
    {
        TaskInstance instance = task.LinkedInstance; // Get the linked task instance
        if (instance == null) return; // If no linked instance, do nothing

        if (task.IsCompleted)
        {
            CompleteTask(instance);
        }
    }
    public void CompleteTask(TaskInstance task)
    {
        //task.Complete();
        // Will remove the task from the list and update UI 
        ActiveTasks.Remove(task);
        taskUIManager?.RemoveTaskUI(task);
    }
}
