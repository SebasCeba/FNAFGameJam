using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskOB : MonoBehaviour
{
    public TaskType taskType;
    private bool playerNerby = false;
    private TaskManager taskManager;
    [SerializeField] private InputActionReference interactAction; // Reference to the interact action
    private void Awake()
    {
        if(taskManager == null)
        {
            taskManager = FindFirstObjectByType<TaskManager>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player")) playerNerby = true;
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player")) playerNerby = false;
    //}
    private void OnEnable()
    {
        interactAction.action.performed += OnInteract; // Subscribe to the interact action
        interactAction.action.Enable(); // Enable the action
    }
    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract; // Unsubscribe from the interact action
        interactAction.action.Disable(); // Disable the action
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!playerNerby || !context.performed) return; 

        var task = taskManager.ActiveTasks.Find(t => t.Type == taskType && !t.IsCompleted);
        if(task != null)
        {
            taskManager.CompleteTask(task);
            Debug.Log($"Task completed via object: {task.Description}");
            Destroy(gameObject);
        }
        Debug.Log("Interact triggered"); 
    }
}
