using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskOB : MonoBehaviour
{
    public TaskType taskType;
    private bool playerNerby = false;
    private TaskManager taskManager; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) playerNerby = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerNerby = false;
    }

    public void OnInterect(InputAction.CallbackContext context)
    {
        if (!playerNerby || !context.performed) return; 

        var task = taskManager.ActiveTasks.Find(t => t.Type == taskType && !t.IsCompleted);
        if(task != null)
        {
            taskManager.CompleteTask(task);
            Debug.Log($"Task completed via object: {task.Description}");
            Destroy(gameObject);
        }
    }
}
