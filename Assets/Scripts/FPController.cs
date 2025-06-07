using UnityEngine;
using Unity.Cinemachine; 

namespace Artemis
{
    [RequireComponent(typeof(CharacterController))]
    public class FPController : MonoBehaviour
    {
        [Header("Movement Parameters")]
        public float MaxSpeed => SprintInput ? SprintSpeed : WalkSpeed;
        public float Acceleration = 15f;

        [SerializeField] float WalkSpeed = 3.5f;
        [SerializeField] public float SprintSpeed = 8f;

        [Space(15)]
        [Tooltip("This is how high the character can jump!")]
        [SerializeField] float JumpHeight = 2f;
        public bool Sprinting
        {
            get
            {
                return SprintInput && CurrentSpeed > 0.1f;
            }
        }

        [Header("Physics Parameters")]
        [SerializeField] float GravityScale = 3f;

        public float VerticalVelocity = 0f;
        public Vector3 CurrentVelocity { get; private set; }
        public float CurrentSpeed { get; private set; }
        public bool IsGrounded => characterController.isGrounded;

        [Header("Inputs")]
        public Vector2 MoveInput;
        public bool SprintInput;

        [Header("Components")]
        [SerializeField] CharacterController characterController;

        [Header("Disable")]
        public bool CanMove = false; //Player shouldn't be able to walk now

        #region Unity Methods 
        void OnValidate()
        {
            if (characterController != null)
            {
                characterController = GetComponent<CharacterController>();
            }
        }

        void Update()
        {
            MoveUpdate();
        }
        #endregion

        #region Controller Methods 
        public void TryJump()
        {
            if(IsGrounded == false)
            {
                return;
            }
            VerticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y * GravityScale); 
        }
        void MoveUpdate()
        {
            if(CanMove == false)
            {
                return;
            }
            // W and S keys are the first half 
            // A and D keys are the second half 
            Vector3 motion = transform.forward * MoveInput.y + transform.right * MoveInput.x;
            motion.y = 0f;
            motion.Normalize();


            if (motion.sqrMagnitude >= 0.01f)
            {
                CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, motion * MaxSpeed, Acceleration * Time.deltaTime);
            }
            else
            {
                CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, Vector3.zero, Acceleration * Time.deltaTime);
            }

            if (IsGrounded && VerticalVelocity <= 0.01f)
            {
                VerticalVelocity = -3f;
            }
            else
            {
                VerticalVelocity += Physics.gravity.y * GravityScale * Time.deltaTime;
            }

            Vector3 fullVelocity = new Vector3(CurrentVelocity.x, VerticalVelocity, CurrentVelocity.z); 

            characterController.Move(fullVelocity * Time.deltaTime); 

            // Updating Speed 
            CurrentSpeed = CurrentVelocity.magnitude;
        }
        #endregion
    }
}
