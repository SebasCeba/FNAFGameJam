using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; 

namespace Artemis
{
    [RequireComponent(typeof(FPController))]
    [RequireComponent(typeof(FPLookController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FPController controller;
        [SerializeField] FPLookController lookController;
        [SerializeField] public CameraManager camManager; 

        #region Input Handling
        void OnMove(InputValue value)
        {
            if(GameManager.gameOver) return;
            controller.MoveInput = value.Get<Vector2>(); 
        }
        void OnLook(InputValue value)
        {
            if (GameManager.gameOver) return;
            lookController.LookInput = value.Get<Vector2>();
        }
        void OnSprint(InputValue value)
        {
            controller.SprintInput = value.isPressed; 
        }
        void OnJump(InputValue value)
        {
            if (GameManager.gameOver) return;
            if(value.isPressed)
            {
                controller.TryJump(); 
            }
        }
        void OnOpenCams(InputValue value)
        {
            if (GameManager.gameOver)
            {
                Cursor.lockState = CursorLockMode.None; // Ensure cursor is free when game is over
                Cursor.visible = true;
                return;
            }
            if (value.isPressed)
            {
                float currentPower = camManager.power.Power; 
                if(currentPower <= 0f)
                {
                    lookController.canLook = true; // To prevent the player from softlocking whenever they have 0 power and try to open the cameras
                    Cursor.lockState = CursorLockMode.Confined; // Ensure cursor is confined when cameras are not open
                    return; // Do not open cameras if power is 0
                }
                camManager.OpenCam();

                // Get current cam state
                bool camsAreOpen = camManager.CamerasOpen;

                lookController.canLook = !camsAreOpen;

                Cursor.lockState = camsAreOpen ? CursorLockMode.None : CursorLockMode.Confined; 
            }
        }
        void OnRebootCams(InputValue value)
        {
            camManager.TryRebootCamera(); 
        }
        #endregion

        #region Unity Methods 

        void OnValidate()
        {
            if(controller == null)
            {
                controller = GetComponent<FPController>();
            }
            if(lookController == null)
            {
                lookController = GetComponent<FPLookController>();
            }
        }
        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        #endregion
    }
}
