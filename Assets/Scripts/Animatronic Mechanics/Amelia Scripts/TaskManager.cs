using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public TaskUIManager taskUIManager; // Reference to the UI manager for tasks
    public List<Task> ActiveTasks = new List<Task>();
    public System.Action OnAnyTaskFailed; 
    private void Update()
    {
        for(int i = ActiveTasks.Count -1; i >= 0; i--)
        {
            var task = ActiveTasks[i];
            float oldTime = task.TimeRemaining; 
            task.UpdateTask(Time.deltaTime);

            if(!task.IsCompleted && oldTime > 0 && task.TimeRemaining <= 0)
            {
                OnAnyTaskFailed?.Invoke();
                ActiveTasks.RemoveAt(i);
                taskUIManager?.RemoveTask(task); // Update UI when a task fails
            }
        }
        //foreach(var task in ActiveTasks)
        //{
        //    float oldTime = task.TimeRemaining;
        //    task.UpdateTask(Time.deltaTime);

        //    if(!task.IsCompleted && oldTime > 0 && task.TimeRemaining <= 0)
        //    {
        //        OnAnyTaskFailed?.Invoke();
        //        ActiveTasks.Remove(task);
        //    }
        //}
    }
    public void AddTask(Task task)
    {
        ActiveTasks.Add(task);
        //Gonna add UI on here 
        taskUIManager?.AddTask(task);
    }
    public void CompleteTask(Task task)
    {
        task.Complete();
        // Will remove the task from the list and update UI 
        ActiveTasks.Remove(task);
        taskUIManager?.RemoveTask(task);
    }
}
