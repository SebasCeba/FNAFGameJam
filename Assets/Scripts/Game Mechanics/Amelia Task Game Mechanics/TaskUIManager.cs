using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskUIManager : MonoBehaviour
{
    [SerializeField] private GameObject taskEntryPrefab; // Prefab for each task entry in the UI
    [SerializeField] private Transform taskListParent; // Where the task entries will be instantiated
    [SerializeField] private int maxTasksOnScreen = 3; // Maximum number of tasks to display at once

    private Dictionary<Task, GameObject> taskToUIEntry = new();

    public void AddTask(Task task)
    {
        if(taskToUIEntry.Count >= maxTasksOnScreen)
        {
            Debug.LogWarning("Maximum number of tasks reached. Cannot add more tasks.");
            return;
        }

        GameObject entry = Instantiate(taskEntryPrefab, taskListParent);
        entry.GetComponentInChildren<TMP_Text>().text = task.Description;

        taskToUIEntry.Add(task, entry);

        task.OnComplete += () => RemoveTask(task);
        task.OnFail += () => RemoveTask(task);
    }
    public void RemoveTask(Task task)
    {
        if(taskToUIEntry.TryGetValue(task, out var entry))
        {
            Destroy(entry);
            taskToUIEntry.Remove(task);
        }
    }
}
