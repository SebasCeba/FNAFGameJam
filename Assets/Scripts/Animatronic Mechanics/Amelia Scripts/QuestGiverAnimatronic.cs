using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;

public class QuestGiverAnimatronic : MonoBehaviour
{
    [SerializeField] private TaskManager taskManager; // Reference to the TaskManager to manage tasks
    [SerializeField] private TaskUIManager taskUIManager;
    [SerializeField] private GameObject taskUIPrefab; // Prefab for the task UI
    [SerializeField] private TaskData[] possibleTasks; 

    public CameraManager camManager; 

    public void GiveTask()
    {
        TaskData chosenTask = possibleTasks[Random.Range(0, possibleTasks.Length)];
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
        //var task = new Task(
        //    "Clean Trash", TaskType.CleanOffice,
        //    onComplete: () => Debug.Log("Diner is cleaned up!"),
        //    onFail: () =>
        //    {
        //        Debug.Log("Failed to clean the diner in time!");
        //        camManager.ForceExitAndLockCameras(); 
        //    }
        //);
        //taskManager.AddTask(task);
    }
    public void GiveSupplyTask()
    {
        Debug.Log("Giving supply task to player...");

        TaskData supplyTask = possibleTasks.FirstOrDefault(t => t.taskName == "Send Supplies");
        if (supplyTask == null)
        {
            taskManager.CreateTask(supplyTask, this);
        }
        //var task = new Task(
        //    "Send Supplies",
        //    TaskType.SendSupplies,
        //    onComplete: () => Debug.Log("Supplies sent!"),
        //    onFail: () =>
        //    {
        //        Debug.Log("Failed to send supplies in time!");
        //        camManager.ForceExitAndLockCameras(); 
        //    }
        //);
        //taskManager.AddTask(task);
    }
    public void GiveDoorTask()
    {
        Debug.Log("Giving door task to player...");
        TaskData doorTask = possibleTasks.FirstOrDefault(t => t.taskName == "Keep Door Open");
        if (doorTask == null)
        {
            taskManager.CreateTask(doorTask, this);
        }
        //var task = new Task(
        //    "Keep Door Open",
        //    TaskType.KeepDoorOpen,
        //    onComplete: () => Debug.Log("Door is kept open!"),
        //    onFail: () =>
        //    {
        //        Debug.Log("Failed to keep the door open in time!");
        //        camManager.ForceExitAndLockCameras();
        //    }
        //);
        //taskManager.AddTask(task);
    }
    public void GiveLightsTask()
    {
        Debug.Log("Giving lights task to player...");
        TaskData lightsTask = possibleTasks.FirstOrDefault(t => t.taskName == "Keep Lights On");
        if (lightsTask == null)
        {
            taskManager.CreateTask(lightsTask, this);
        }
        //var task = new Task(
        //    "Keep Lights On",
        //    TaskType.KeepLightsOn,
        //    onComplete: () => Debug.Log("Lights are kept on!"),
        //    onFail: () =>
        //    {
        //        Debug.Log("Failed to keep the lights on in time!");
        //        camManager.ForceExitAndLockCameras(); 
        //    }
        //);
        //taskManager.AddTask(task);
    }
}
