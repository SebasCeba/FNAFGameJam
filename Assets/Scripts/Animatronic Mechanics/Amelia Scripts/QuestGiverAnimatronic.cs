using System.Linq;
using System.Collections.Generic;
using System.Collections; 
using UnityEngine;

public class QuestGiverAnimatronic : MonoBehaviour
{
    [Header("Task Settings")]
    public float taskInterval = 60f; // Time between tasks in seconds, can be adjusted in the inspector
    public float taskTimeLimit = 45f; // Time limit for each task in seconds, can be adjusted in the inspector
    public int maxTasks = 3; // Maximum number of tasks that can be active at once

    private List<GameObject> trashObj = new List<GameObject>(); // List to hold trash objects for the cleaning task
    private List<GameObject> supplyObj = new List<GameObject>(); // List to hold supply objects for the supply task
    private List<Door> door = new List<Door>(); // List to hold door objects for the door task
    private List<Door> lights = new List<Door>(); // List to hold lights objects for the lights task

    private int currentTaskCount = 0; // Counter for the number of tasks given

    [SerializeField] private TaskManager taskManager; // Reference to the TaskManager to manage tasks
    [SerializeField] private TaskUIManager taskUIManager;
    [SerializeField] private GameObject taskUIPrefab; // Prefab for the task UI
    [SerializeField] public TaskData[] possibleTasks; 

    public CameraManager camManager;

    public float doorTimeOpen;
    public float lightTimeOn; 

    private void Start()
    {
        //taskManager.OnAnyTaskFailed += () => camManager.ForceExitAndLockCameras(); // Subscribe to task failure event to lock cameras
        //GiveTask(); // Give a task when the script starts
        ScanSceneObjects(); // Scan the scene for objects related to tasks
        StartCoroutine(TaskLoop()); // Start the task loop coroutine to generate tasks periodically
    }
    //public void GiveTask()
    //{
    //    if (possibleTasks == null || possibleTasks.Length == 0)
    //    {
    //        Debug.LogError("[QuestGiver] No possible tasks assigned!");
    //        return;
    //    }

    //    TaskData chosenTask = possibleTasks[Random.Range(0, possibleTasks.Length)];
    //    if (chosenTask == null)
    //    {
    //        Debug.LogError("[QuestGiver] Chosen task is null!");
    //        return;
    //    }

    //    Debug.Log($"[QuestGiver] Giving task: {chosenTask.taskName}");
    //    taskManager.CreateTask(chosenTask, this); 
    //}
    public GameObject GetTaskUIPrefab()
    {
        return taskUIPrefab;
    }
    //public void GiveCleaningTask()
    //{
    //    Debug.Log("Giving cleaning task to player...");

    //    // Find the cleaning task in poissible tasks 
    //    TaskData cleaningtask = possibleTasks.FirstOrDefault(t => t.taskName == "Clean Trask");
    //    if (cleaningtask == null)
    //    {
    //        taskManager.CreateTask(cleaningtask, this);
    //    }
    //}
    //public void GiveSupplyTask()
    //{
    //    Debug.Log("Giving supply task to player...");

    //    TaskData supplyTask = possibleTasks.FirstOrDefault(t => t.taskName == "Send Supplies");
    //    if (supplyTask == null)
    //    {
    //        taskManager.CreateTask(supplyTask, this);
    //    }
    //}
    //public void GiveDoorTask()
    //{
    //    Debug.Log("Giving door task to player...");
    //    TaskData doorTask = possibleTasks.FirstOrDefault(t => t.taskName == "Keep Door Open");
    //    if (doorTask == null)
    //    {
    //        taskManager.CreateTask(doorTask, this);
    //    }
    //}
    //public void GiveLightsTask()
    //{
    //    Debug.Log("Giving lights task to player...");
    //    TaskData lightsTask = possibleTasks.FirstOrDefault(t => t.taskName == "Keep Lights On");
    //    if (lightsTask == null)
    //    {
    //        taskManager.CreateTask(lightsTask, this);
    //    }
    //}
    private void ScanSceneObjects()
    {
        trashObj.Clear();
        supplyObj.Clear();
        door.Clear();
        lights.Clear();

        trashObj.AddRange(GameObject.FindGameObjectsWithTag("Trash")); // Find all trash objects in the scene
        supplyObj.AddRange(GameObject.FindGameObjectsWithTag("Supply")); // Find all supply objects in the scene
        
        // Dind all Door script tagged "DoorButton" 
        foreach(var obj in GameObject.FindGameObjectsWithTag("DoorButton"))
        {
            var doors = obj.GetComponent<Door>();
            if(doors != null) door.Add(doors); // Add the door to the list if it has a Door component
        }
        // Find all Door script tagged "LightButton"
        foreach(var obj in GameObject.FindGameObjectsWithTag("LightButton"))
        {
            var light = obj.GetComponent<Door>();
            if (light != null) lights.Add(light); // Add the light to the list if it has a Door component
        }
    }
    private IEnumerator TaskLoop()
    {
        while (true)
        {
            if (currentTaskCount < maxTasks) // Check if the current task count is less than the maximum allowed tasks
            {
                GenerateRandomTask(); // Generate a new task
            }
            yield return new WaitForSeconds(taskInterval); // Wait for the specified interval before generating the next task
        }
    }
    private void GenerateRandomTask()
    {
        int taskType = Random.Range(0, 4); // Randomly select a task type (0-3)
        TaskData chosenTask = null; 
        switch(taskType)
        {
            case 0: // Trash 
                chosenTask = possibleTasks.FirstOrDefault(t => t.taskType == TaskType.CleanOffice);
                ActivateObjects(trashObj, true); // Activate all trash objects
                StartCoroutine(TaskTimer(trashObj, "Trash")); // Start the task timer for trash
                break;
            case 1: // Supply
                chosenTask = possibleTasks.FirstOrDefault(t => t.taskType == TaskType.SendSupplies);
                ActivateObjects(supplyObj, true); // Activate all supply objects
                StartCoroutine(TaskTimer(supplyObj, "Supply")); // Start the task timer for supply
                break;
            case 2: // Door
                chosenTask = possibleTasks.FirstOrDefault(t => t.taskType == TaskType.KeepDoorOpen);
                StartCoroutine(DoorTaskTimer());
                break;
            case 3: // Lights
                chosenTask = possibleTasks.FirstOrDefault(t => t.taskType == TaskType.KeepLightsOn);
                StartCoroutine(LightTaskTimer());
                break;
        }
        if (chosenTask != null)
            taskManager.CreateTask(chosenTask, this);
        currentTaskCount++; // Increment the task count
    }
    private IEnumerator TaskTimer(List<GameObject> objects, string tag)
    {
        float timer = taskTimeLimit; // Set the timer to the task time limit
        while (timer > 0)
        {
            if(AllObjectsInactive(objects))
            {
                Debug.Log($"[QuestGiver] All {tag} objects are inactive. Task completed.");
                currentTaskCount--;
                yield break;
            }
            timer -= Time.deltaTime; // Decrease the timer by the time since last frame
            yield return null; // Wait for the next frame
        }
        // Task Failed 
        Debug.Log($"Task '{tag}' failed!");
        ActivateObjects(objects, false); // Deactivate all objects related to the task
        currentTaskCount--; // Decrement the task count
        camManager.ForceExitAndLockCameras(); // Lock cameras if the task fails
    }
    private IEnumerator DoorTaskTimer()
    {
        float timer = taskTimeLimit; // Set the timer to the task time limit
        float requiredOpenTime = doorTimeOpen; // Seconds  the door must stay open 
        float openTime = 0f; 
        while (timer > 0)
        {
            bool allOpen = true; 
            foreach(var doors in door)
            {
                if(!doors.IsOpen) { allOpen = false; break; } // Check if all doors are open
            }
            if (allOpen)
            {
                openTime += Time.deltaTime; // Increment the open time if all doors are open
                if (openTime >= requiredOpenTime) // Check if the doors have been open for the required time
                {
                    Debug.Log("[QuestGiver] All doors are open for the required time. Task completed.");
                    currentTaskCount--; // Decrement the task count
                    yield break; // Exit the coroutine if all doors are open for the required time
                }
            }
            else
            {
                openTime = 0f; // Reset the open time if any door is closed
            }
            timer -= Time.deltaTime; // Decrease the timer by the time since last frame
            yield return null; // Wait for the next frame
        }
        Debug.Log("Door task failed!"); // Log task failure
        currentTaskCount--; // Decrement the task count
        camManager.ForceExitAndLockCameras(); // Lock cameras if the task fails
    }
    private IEnumerator LightTaskTimer()
    {
        float timer = taskTimeLimit;
        float requiredOnTime = lightTimeOn; // Seconds the lights must stay on
        float openTime = 0f; 
        while (timer > 0)
        {
            bool allOn = true; // Assume all lights are on
            foreach (var light in lights)
            {
                if (!light.IsOn) { allOn = false; break; } // Check if all lights are on
            }
            if (allOn)
            {
                openTime += Time.deltaTime; // Increment the open time if all lights are on
                if (openTime >= requiredOnTime)
                {
                    Debug.Log("[QuestGiver] All lights are on for the required time. Task completed.");
                    currentTaskCount--; // Decrement the task count
                    yield break; // Exit the coroutine if all lights are on for the required time
                }
            }
            else
            {
                openTime = 0f; // Reset the open time if any light is off
            }
            timer -= Time.deltaTime; // Decrease the timer by the time since last frame
            yield return null; // Wait for the next frame
        }
        Debug.Log("Lights task failed!"); // Log task failure
        currentTaskCount--; // Decrement the task count
        camManager.ForceExitAndLockCameras(); // Lock cameras if the task fails
    }
    private void ActivateObjects(List<GameObject> objects, bool state)
    {
        foreach(var obj in objects)
        {
            obj.SetActive(state); // Activate or deactivate the object based on the state
        }
    }
    private bool AllObjectsInactive(List<GameObject> objects)
    {
        foreach(var obj in objects)
        {
            if (obj.activeSelf) return false; // If any object is active, return false
        }
        return true; 
    }
}
