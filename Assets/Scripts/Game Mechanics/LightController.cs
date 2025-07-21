using UnityEngine;
using UnityEngine.InputSystem;

public class LightController : MonoBehaviour
{
    [SerializeField] private Door door;
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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                door.ChangeLight(); // Call the ChangeLight method on the Door script
                Debug.Log($"Light state toggled. IsOn: {door.IsOn}"); // Log the state change
            }
        }
    }
}
