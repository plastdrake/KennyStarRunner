using UnityEngine;
using UnityEngine.InputSystem;

public class RunnerCharacterController : MonoBehaviour
{
    public float forwardSpeed = 8f;
    public float sideSpeed = 5f;
    public float jumpForce = 200f;
    public float doubleJumpForce = 50f;
    public float gravity = 15f;
    public float turnSpeed = 10f; // For smooth turning

    private float verticalVelocity;
    private bool canDoubleJump = false;
    private bool jumpRequest = false;
    private bool doubleJumpRequest = false;

    private CharacterController controller;
    private Animator animator;

    private Vector3 moveDirection = Vector3.forward;
    private Quaternion targetRotation;

    private Transform camTransform;
    private Vector3 camDefaultLocalPos;
    private Quaternion camDefaultLocalRot;
    public Vector3 camFrontLocalPos = new Vector3(0, 2, 3); // Adjust as needed
    public Vector3 camFrontLocalEuler = new Vector3(10, 180, 0); // Adjust as needed

    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private bool cameraReverseHeld = false;

    void Awake()
    {
        inputActions = new InputSystem_Actions();

        // Movement input
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Jump input
        inputActions.Player.Jump.performed += ctx => OnJump();

        // Turn left/right input
        inputActions.Player.TurnLeft.performed += ctx => OnTurnLeft();
        inputActions.Player.TurnRight.performed += ctx => OnTurnRight();

        // Camera reverse input (hold)
        inputActions.Player.CameraReverse.performed += ctx => cameraReverseHeld = true;
        inputActions.Player.CameraReverse.canceled += ctx => cameraReverseHeld = false;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        moveDirection = Vector3.forward;
        targetRotation = transform.rotation;

        camTransform = GetComponentInChildren<Camera>().transform;
        camDefaultLocalPos = camTransform.localPosition;
        camDefaultLocalRot = camTransform.localRotation;
    }

    void Update()
    {
        // Always move forward
        float speed = forwardSpeed;
        animator.SetFloat("Speed", speed);

        // Free side-to-side movement (relative to current forward)
        float horizontalInput = moveInput.x; // -1 (left), 0 (center), 1 (right)
        animator.SetFloat("Strafe", horizontalInput);

        // Camera reverse while holding CameraReverse action (e.g., V)
        if (cameraReverseHeld)
        {
            camTransform.localPosition = camFrontLocalPos;
            camTransform.localEulerAngles = camFrontLocalEuler;
        }
        else
        {
            camTransform.localPosition = camDefaultLocalPos;
            camTransform.localRotation = camDefaultLocalRot;
        }

        // Smoothly rotate character (and camera, if childed) to new direction
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        bool isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            animator.SetBool("IsInAir", false);
            canDoubleJump = true;

            if (jumpRequest)
            {
                verticalVelocity = jumpForce;
                animator.SetTrigger("Jump");
                animator.ResetTrigger("Jump");
                animator.SetBool("IsInAir", true);
                jumpRequest = false;
            }
        }
        else
        {
            animator.SetBool("IsInAir", true);
            verticalVelocity -= gravity * Time.deltaTime;

            if (doubleJumpRequest && canDoubleJump)
            {
                verticalVelocity = doubleJumpForce;
                animator.SetTrigger("DoubleJump");
                canDoubleJump = false;
                doubleJumpRequest = false;
            }
        }

        // Calculate movement vector in the new forward direction
        Vector3 right = Vector3.Cross(Vector3.up, moveDirection); // right relative to current forward
        Vector3 moveVector = (moveDirection * speed) + (right * horizontalInput * sideSpeed) + (Vector3.up * verticalVelocity);
        controller.Move(moveVector * Time.deltaTime);
    }

    private void OnJump()
    {
        if (controller.isGrounded)
            jumpRequest = true;
        else if (canDoubleJump)
            doubleJumpRequest = true;
    }

    private void OnTurnLeft()
    {
        moveDirection = Quaternion.Euler(0, -90, 0) * moveDirection;
        targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
    }

    private void OnTurnRight()
    {
        moveDirection = Quaternion.Euler(0, 90, 0) * moveDirection;
        targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
    }
}
