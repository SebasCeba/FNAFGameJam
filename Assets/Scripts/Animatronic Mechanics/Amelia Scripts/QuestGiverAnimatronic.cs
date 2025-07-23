using UnityEngine;

public class QuestGiverAnimatronic : MonoBehaviour
{
    public TaskManager taskManager; // Reference to the TaskManager to manage tasks
    public CameraManager camManager; 

    public void GiveCleaningTask()
    {
        var task = new Task(
            "Clean Trash",
            TaskType.CleanOffice,
            onComplete: () => Debug.Log("Diner is cleaned up!"),
            onFail: () =>
            {
                Debug.Log("Failed to clean the diner in time!");
                camManager.ForceExitAndLockCameras(); // Disable camera 1 as a consequence
            }
        );
        taskManager.AddTask(task);
        // Update the UI to notify the player's duties. 
    }
    public void GiveSupplyTask()
    {
        var task = new Task(
            "Send Supplies",
            TaskType.SendSupplies,
            onComplete: () => Debug.Log("Supplies sent!"),
            onFail: () =>
            {
                Debug.Log("Failed to send supplies in time!");
                camManager.ForceExitAndLockCameras(); // Disable camera 2 as a consequence
            }
        );
        taskManager.AddTask(task);
    }
    public void GiveDoorTask()
    {
        var task = new Task(
            "Keep Door Open",
            TaskType.KeepDoorOpen,
            onComplete: () => Debug.Log("Door is kept open!"),
            onFail: () =>
            {
                Debug.Log("Failed to keep the door open in time!");
                camManager.ForceExitAndLockCameras(); // Disable camera 3 as a consequence
            }
        );
        taskManager.AddTask(task);
    }
    public void GiveLightsTask()
    {
        var task = new Task(
            "Keep Lights On",
            TaskType.KeepLightsOn,
            onComplete: () => Debug.Log("Lights are kept on!"),
            onFail: () =>
            {
                Debug.Log("Failed to keep the lights on in time!");
                camManager.ForceExitAndLockCameras(); // Disable camera 4 as a consequence
            }
        );
        taskManager.AddTask(task);
    }
}
