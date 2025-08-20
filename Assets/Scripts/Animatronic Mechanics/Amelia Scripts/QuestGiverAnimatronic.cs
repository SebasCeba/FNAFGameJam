using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;

public class QuestGiverAnimatronic : MonoBehaviour
{
    [SerializeField] private TaskManager taskManager; // Reference to the TaskManager to manage tasks
    [SerializeField] private TaskUIManager taskUIManager;
    [SerializeField] private GameObject taskUIPrefab; // Prefab for the task UI
    [SerializeField] public TaskData[] possibleTasks; 

    public CameraManager camManager;

    private void Start()
    {
        taskManager.OnAnyTaskFailed += () => camManager.ForceExitAndLockCameras(); // Subscribe to task failure event to lock cameras
        GiveTask(); // Give a task when the script starts
    }
    public void GiveTask()
    {
        if (possibleTasks == null || possibleTasks.Length == 0)
        {
            Debug.LogError("[QuestGiver] No possible tasks assigned!");
            return;
        }

        TaskData chosenTask = possibleTasks[Random.Range(0, possibleTasks.Length)];
        if (chosenTask == null)
        {
            Debug.LogError("[QuestGiver] Chosen task is null!");
            return;
        }

        Debug.Log($"[QuestGiver] Giving task: {chosenTask.taskName}");
        taskManager.CreateTask(chosenTask, this); 
    }
    public GameObject GetTaskUIPrefab()
    {
        return taskUIPrefab;
    }
    public void GiveCleaningTask()
    {
        Debug.Log("Giving cleaning task to player...");

        // Find the cleaning task in poissible tasks 
        TaskData cleaningtask = possibleTasks.FirstOrDefault(t => t.taskName == "Clean Trask");
        if (cleaningtask == null)
        {
            taskManager.CreateTask(cleaningtask, this);
        }
    }
    public void GiveSupplyTask()
    {
        Debug.Log("Giving supply task to player...");

        TaskData supplyTask = possibleTasks.FirstOrDefault(t => t.taskName == "Send Supplies");
        if (supplyTask == null)
        {
            taskManager.CreateTask(supplyTask, this);
        }
    }
    public void GiveDoorTask()
    {
        Debug.Log("Giving door task to player...");
        TaskData doorTask = possibleTasks.FirstOrDefault(t => t.taskName == "Keep Door Open");
        if (doorTask == null)
        {
            taskManager.CreateTask(doorTask, this);
        }
    }
    public void GiveLightsTask()
    {
        Debug.Log("Giving lights task to player...");
        TaskData lightsTask = possibleTasks.FirstOrDefault(t => t.taskName == "Keep Lights On");
        if (lightsTask == null)
        {
            taskManager.CreateTask(lightsTask, this);
        }
    }
}
