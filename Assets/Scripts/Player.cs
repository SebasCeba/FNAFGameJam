using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; 

namespace Artemis
{
    [RequireComponent(typeof(FPController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FPController controller;

        #region Input Handling
        void OnMove(InputValue value)
        {
            controller.MoveInput = value.Get<Vector2>(); 
        }
        void OnLook(InputValue value)
        {
            controller.LookInput = value.Get<Vector2>();
        }
        void OnSprint(InputValue value)
        {
            controller.SprintInput = value.isPressed; 
        }
        void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                controller.TryJump(); 
            }
        }
        #endregion

        #region Unity Methods 

        void OnValidate()
        {
            if(controller == null)
            {
                controller = GetComponent<FPController>();
            }
        }
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion
    }
}
