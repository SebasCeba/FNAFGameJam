using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskOB : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction; // Reference to the interact action
    private Camera mainCam;
    private bool isActiveTask;
    public bool IsCompleted { get; private set; } = false; //  for TaskManager
    public string TaskDescription { get; private set; }   //  UI info
    public float TimeLimit { get; private set; }          //  Timer info
    public TaskInstance LinkedInstance { get; private set; }
    private void OnEnable()
    {
        interactAction.action.performed += OnInteract; // Subscribe to the interact action
        interactAction.action.Enable(); // Enable the action
        isActiveTask = true; // Set the task as active when enabled
        IsCompleted = false; 
        Debug.Log($"[TaskObject] Task activated: {gameObject.name}");
    }
    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract; // Unsubscribe from the interact action
        //interactAction.action.Disable(); // Disable the action
        isActiveTask = false; // Set the task as inactive when disabled
        Debug.Log($"[TaskObject] Task deactivated: {gameObject.name}");
    }
    private void Start()
    {
        mainCam = Camera.main; // Get the main camera reference
    }
    public void Setup(TaskInstance instance)
    {
        LinkedInstance = instance; // Link the task instance
        
        TaskDescription = instance.Data.taskName; // Set the task description
        TimeLimit = instance.Data.timeLimit; // Set the time limit for the task 

        IsCompleted = false; // Reset completion status
        gameObject.SetActive(true); // Ensure the task object is active
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(!isActiveTask) return; // Check if the task is active

        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue()); // Cast a ray from the mouse position
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.transform == transform) // Check if the ray hit this object
            {
                Debug.Log($"[TaskObject] Interacted with: {gameObject.name}"); // Log the interaction
                CompleteTask(); 
            }
        }
    }
    private void CompleteTask()
    {
        if(IsCompleted) return; // Check if the task is already completed
        IsCompleted = true; // Mark the task as completed
        Debug.Log($"[TaskObject] Task completed: {gameObject.name}"); // Log the task completion
        gameObject.SetActive(false); // Deactivate the task object
    }
}
