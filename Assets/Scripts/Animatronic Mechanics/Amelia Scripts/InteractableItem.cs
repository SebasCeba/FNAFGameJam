using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableItem : MonoBehaviour
{
    public TaskType taskType; // Type of the task this item is associated with
    public string taskKeyword; // Name of the item
    private bool playerNearby = false;

    private TaskManager taskManager;
    private void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) playerNearby = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerNearby = false;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!playerNearby || !context.performed) return;

        var task = taskManager.ActiveTasks.Find(t =>
        t.Data != null &&
        t.taskType == taskType &&
        t.Data.taskName.Contains(taskKeyword)); /*== taskType && t.Description.Contains(taskKeyword));*/

        if(task != null)
        {
            taskManager.CompleteTask(task); // Complete the task if it exists
            Destroy(gameObject); // Destroy the item after interaction
            Debug.Log($"Interacted with {taskKeyword} and completed the task.");
        }
    }
}
