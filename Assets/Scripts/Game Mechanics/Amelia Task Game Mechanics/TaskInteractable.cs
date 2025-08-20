using UnityEngine;
using UnityEngine.InputSystem;

// Represents a physical object in the scene that can be interacted with for a task. 
public class TaskInteractable : MonoBehaviour
{
    [Header("Task Properties")]
    public TaskType taskType; // Tpype of task this interactable represents
    public string taskKeyword; // Keyword to identify the task
    [SerializeField] private InputActionReference interactAction; // Reference to the interact action

    private bool playerNearby = false; // Flag to check if player is nearby
    private Camera mainCam;
    private bool isActiveTask;
    public bool IsCompleted { get; private set; } = false; // For TaskManager
    public TaskInstance LinkedInstance { get; private set; } // Linked task instance
    private TaskManager taskManager; // Reference to the TaskManager

    // Setup and event registration
    private void OnEnable()
    {
        interactAction.action.performed += OnInteract; // Subscribe to the interact action
        interactAction.action.Enable(); // Enable the action
        isActiveTask = true; // Set the task as active when enabled
        IsCompleted = false;
    }
    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract; // Unsubscribe from the interact action
        //interactAction.action.Disable(); // Disable the action
        isActiveTask = false; // Set the task as inactive when disabled
    }
    private void Start()
    {
       mainCam = Camera.main; // Get the main camera reference
        taskManager = FindFirstObjectByType<TaskManager>(); // Find the TaskManager in the scene
    }

    // Link this object to a task instance
    public void Setup(TaskInstance instance)
    {
        LinkedInstance = instance; // Link the task instance
        IsCompleted = false; // Reset completion status
        gameObject.SetActive(true); // Ensure the task object is active
    }

    // Player enters/exists the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerNearby = true; // Player is nearby
            Debug.Log($"[TaskInteractable] Player entered trigger: {gameObject.name}");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false; // Player is no longer nearby
            Debug.Log($"[TaskInteractable] Player exited trigger: {gameObject.name}");
        }
    }

    // Handle interaction input
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!isActiveTask || !playerNearby || !context.performed) return; // Check if the task is active and player is nearby
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue()); // Cast a ray from the mouse position
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform) // Check if the ray hit this object
            {
                if(hit.transform == transform)
                {
                    Debug.Log($"[TaskInteractable] Interacted with task: {gameObject.name}");
                    taskManager.ReportObjectCompleted(this); // Report the task completion to the TaskManager
                    CompleteTask(); // Complete the task
                }
            }
        }
    }

    // Complete the task and perform necessary actions
    private void CompleteTask()
    {
        if(IsCompleted) return; // Check if the task is already completed
        IsCompleted = true; // Mark the task as completed
        Debug.Log($"[TaskInteractable] Task completed: {gameObject.name}"); // Log the completion
        gameObject.SetActive(false); // Optionally deactivate the object
    }
}
