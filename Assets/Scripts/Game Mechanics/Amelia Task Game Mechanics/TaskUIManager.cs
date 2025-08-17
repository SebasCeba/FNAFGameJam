using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskUIManager : MonoBehaviour
{
    [SerializeField] private Transform taskListParent; // Where the task entries will be instantiated
    
    private Queue<TaskUIEntry> pool = new Queue<TaskUIEntry>();
    public void AddTaskUI(TaskInstance instance, GameObject prefab)
    {
        TaskUIEntry entry = GetFromPool(prefab); 
        entry.Setup(instance.Data.taskName, instance.Data.numberToSpawn, instance.Data.timeLimit, instance.Data.uiFormat);
        entry.gameObject.SetActive(true); // Activate the UI entry

        instance.UIEntry = entry; 
    }
    public void RemoveTaskUI(TaskInstance instance)
    {
        if(instance.UIEntry != null)
        {
            instance.UIEntry.gameObject.SetActive(false); // Deactivate the UI entry
            pool.Enqueue(instance.UIEntry); // Return the entry to the pool
            instance.UIEntry = null; // Clear the reference in the task instance
        }
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
