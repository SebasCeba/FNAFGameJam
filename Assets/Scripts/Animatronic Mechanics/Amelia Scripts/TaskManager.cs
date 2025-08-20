using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Manages all active tasks, their creation, completion and UI updates. 
public class TaskManager : MonoBehaviour
{
    public int maxTasks = 3; // Maximum number of tasks that can be active at once
    public TaskUIManager taskUIManager; // Reference to the UI manager for tasks
    public List<TaskInstance> ActiveTasks = new List<TaskInstance>(); // :ist of currently active tasks
    public System.Action OnAnyTaskFailed; // Event for when any task fails
    public void CreateTask(TaskData data, QuestGiverAnimatronic giver, System.Action onComplete = null, System.Action onFail = null)
    {
        if(ActiveTasks.Count >= maxTasks)
        {
            Debug.LogWarning("[TaskManager] Maximum number of active tasks reached. Cannot create new task.");
            return; // Prevent creating a new task if the limit is reached
        }

        Debug.Log($"[TaskManager] Creating task from data: {data.taskName}");

        //Activate relevant objects in the scene
        foreach(var obj in FindObjectsByType<TaskInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if(obj.taskType == data.taskType && data.taskName.Contains(obj.taskKeyword))
            {
                obj.Setup(new TaskInstance(data, onComplete, onFail)); // Setup the task interactable with the new task instance
                obj.gameObject.SetActive(true); // Ensure the task interactable is active
            }
        }
        TaskInstance instance = new TaskInstance(data, onComplete, onFail); // Create a new task instance with the data and empty list of spawned objects
        ActiveTasks.Add(instance);
        taskUIManager.AddTaskUI(instance, giver.GetTaskUIPrefab());
    }
    public void ReportObjectCompleted(TaskInteractable task)
    {
        // Find the matching active task 
        var instance = ActiveTasks.Find(t => 
        t.Data.taskType == task.taskType && 
        t.Data.taskName.Contains(task.taskKeyword));

        if (instance != null) return; 

        instance.CompletedCount++;
        taskUIManager.UpdateTaskUI(instance); 
    }
    public void CompleteTask(TaskInstance task)
    {
        ActiveTasks.Remove(task);
        taskUIManager?.RemoveTaskUI(task);

        // Deactive related objects 
        foreach (var obj in FindObjectsByType<TaskInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if(obj.taskType == task.Data.taskType && task.Data.taskName.Contains(obj.taskKeyword))
            {
                obj.gameObject.SetActive(false); // Deactivate the task interactable
            }
        }
    }
}
