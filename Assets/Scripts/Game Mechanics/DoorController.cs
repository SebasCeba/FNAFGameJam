using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Door door; // Reference to the Door script
    [SerializeField] private InputActionReference interactAction; // Assign in Inspector

    private Camera mainCam;

    private void OnEnable()
    {
        interactAction.action.performed += OnInteract;
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
    }
    private void Start()
    {
        mainCam = Camera.main; 
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Cast a ray from the mouse position to the scene 
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                door.IsOpen = !door.IsOpen; // Toggle the door state
                Debug.Log($"Door state toggled. IsOpen: {door.IsOpen}"); // Log the state change 
            }
        }
    }
}
