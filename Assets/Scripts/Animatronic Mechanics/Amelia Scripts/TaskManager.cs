using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<Task> ActiveTasks = new List<Task>();

    private void Update()
    {
        foreach(var task in ActiveTasks)
        {
            task.UpdateTask(Time.deltaTime);
        }
    }
    public void AddTask(Task task)
    {
        ActiveTasks.Add(task);
        //Gonna add UI on here 
    }
    public void CompleteTask(Task task)
    {
        task.Complete();
        // Will remove the task from the list and update UI 
        ActiveTasks.Remove(task);
    }
}
