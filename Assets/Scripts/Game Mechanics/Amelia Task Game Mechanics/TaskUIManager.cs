using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskUIManager : MonoBehaviour
{
    //[SerializeField] private GameObject taskEntryPrefab; // Prefab for each task entry in the UI
    [SerializeField] private Transform taskListParent; // Where the task entries will be instantiated
    //[SerializeField] private int maxTasksOnScreen = 3; // Maximum number of tasks to display at once

    //private Dictionary<Task, GameObject> taskToUIEntry = new();
    //private Queue<GameObject> uiPool = new();

    private Queue<TaskUIEntry> pool = new Queue<TaskUIEntry>();
    
    //private void Start()
    //{
    //    // Pre-Instantiate pooled objects 
    //    for(int i = 0; i < maxTasksOnScreen; i++)
    //    {
    //        var entry = Instantiate(taskEntryPrefab, taskListParent);
    //        entry.SetActive(false); // Initially inactive
    //        uiPool.Enqueue(entry);
    //    }
    //}
    public void AddTaskUI(TaskInstance instance, GameObject prefab)
    {
        TaskUIEntry entry = GetFromPool(prefab); 
        entry.Setup(instance.Data.taskName, instance.Data.numberToSpawn, instance.Data.timeLimit, instance.Data.uiFormat);
        entry.gameObject.SetActive(true); // Activate the UI entry

        instance.UIEntry = entry; 
        //if(taskToUIEntry.Count >= maxTasksOnScreen || uiPool.Count == 0)
        //{
        //    Debug.LogWarning("Maximum number of tasks reached. Cannot add more tasks.");
        //    return;
        //}

        //GameObject entry = uiPool.Dequeue();
        //entry.SetActive(true);
        //entry.GetComponentInChildren<TextMeshProUGUI>().text = instance.Description; // Assuming the prefab has a TextMeshProUGUI component for the description

        //taskToUIEntry[instance] = entry;

        //taskToUIEntry.Add(instance, entry);

        //instance.OnComplete += () => RemoveTask(instance);
        //instance.OnFail += () => RemoveTask(instance);

        //Debug.Log($"Task added: {instance.Description}"); 
    }
    public void RemoveTaskUI(TaskInstance instance)
    {
        if(instance.UIEntry != null)
        {
            instance.UIEntry.gameObject.SetActive(false); // Deactivate the UI entry
            pool.Enqueue(instance.UIEntry); // Return the entry to the pool
            instance.UIEntry = null; // Clear the reference in the task instance
        }
        //if(taskToUIEntry.TryGetValue(task, out var entry))
        //{
        //    entry.SetActive(false); // Deactivate the UI entry
        //    uiPool.Enqueue(entry); // Return the entry to the pool
        //    taskToUIEntry.Remove(task);

        //    Debug.Log($"Task removed: {task.Description}");
        //}
    }
    private TaskUIEntry GetFromPool(GameObject prefab)
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(prefab, taskListParent);
            return obj.GetComponent<TaskUIEntry>();
        }
    }
}
