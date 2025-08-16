using UnityEngine;

public class TaskData
{
    [Header("General Info")]
    public string taskName; // "Clean this Office" , "Send Supplies", "Keep Door Open", "Keep Lights On"
    public string uiFormat; // { Number of Tasks/Items } Items : { Time Remaining}s" 
    public GameObject objectPrefab; // Prefab for the task item, e.g. trash, supplies, etc.
    public Transform[] spawnPoints; // Some tasks may require spawn points for items or objectives

    [Header("Task Settings")]
    public int numberToSpawn; // Used for "clean/send supplies" tasks, how many items to spawn
    public float timeLimit; // Time limit for the task in seconds

    [Header("Special Types")]
    public bool requiresHold; // Door/Lights tasks may require holding an action
}
