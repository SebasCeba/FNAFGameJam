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
        [SerializeField] CameraManager camManager; 

        #region Input Handling
        void OnMove(InputValue value)
        {
            controller.MoveInput = value.Get<Vector2>(); 
        }
        void OnLook(InputValue value)
        {
            lookController.LookInput = value.Get<Vector2>();
        }
        void OnSprint(InputValue value)
        {
            controller.SprintInput = value.isPressed; 
        }
        void OnJump(InputValue value)
        {
            if(value.isPressed)
            {
                controller.TryJump(); 
            }
        }
        //void OnInteract(InputValue value)
        //{
        //    if(value.isPressed)
        //    {
        //        camManager.SwitchCams();
        //    }
        //}
        void OnOpenCams(InputValue value)
        {
            if(value.isPressed)
            {
                camManager.OpenCam();

                // Get current cam state
                bool camsAreOpen = camManager.CamerasOpen;

                lookController.canLook = !camsAreOpen;

                Cursor.lockState = camsAreOpen ? CursorLockMode.None : CursorLockMode.Confined; 
            }
            //else
            //{
            //    camManager.OpenCam();
            //    lookController.canLook = true;
            //}
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
