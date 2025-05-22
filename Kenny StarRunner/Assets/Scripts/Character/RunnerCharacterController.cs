using UnityEngine;
using UnityEngine.InputSystem;

public class RunnerCharacterController : MonoBehaviour
{
    public float forwardSpeed = 8f;
    public float strafeSpeedMultiplier = 0.6f; // Strafe speed as a multiple of forward speed
    public float jumpForce = 200f;
    public float doubleJumpForce = 50f;
    public float gravity = 15f;
    public float turnSpeed = 10f; // For smooth turning

    public float runAnimationAuthoredSpeed = 8f; // Set this to your run animation's intended speed
    public bool IsFreeRunning = false; // Toggle for free running mode

    // Slide mechanic fields
    public float slideBoost = 4f;           // Extra speed during slide
    public float slideDuration = 0.7f;      // How long the slide lasts
    public float slideColliderHeight = 1f;  // Height of collider during slide

    [SerializeField] private Transform meshRoot; // Assign in Inspector to your Armature
    private Vector3 meshRootDefaultLocalPos;
    private Quaternion meshRootDefaultLocalRot;

    private bool isSliding = false;
    private float slideTimer = 0f;
    private float originalColliderHeight;
    private float originalColliderCenterY;

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
    private float forwardInput = 0f;
    private bool cameraReverseHeld = false;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        moveInput = Vector2.zero;
        forwardInput = 0f;

        // Movement input
        inputActions.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
            forwardInput = moveInput.y;
        };
        inputActions.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
            forwardInput = 0f;
        };

        // Jump input
        inputActions.Player.Jump.performed += ctx => OnJump();

        // Turn left/right input
        inputActions.Player.TurnLeft.performed += ctx => OnTurnLeft();
        inputActions.Player.TurnRight.performed += ctx => OnTurnRight();

        // Camera reverse input (hold)
        inputActions.Player.CameraReverse.performed += ctx => cameraReverseHeld = true;
        inputActions.Player.CameraReverse.canceled += ctx => cameraReverseHeld = false;

        // Slide (Sprint) input
        inputActions.Player.Sprint.performed += ctx => StartSlide();
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

        // Ensure animator starts in idle
        animator.SetFloat("Speed", 0f);
        animator.SetBool("IsInAir", false);
        animator.speed = 1f;

        // Store original collider values
        originalColliderHeight = controller.height;
        originalColliderCenterY = controller.center.y;

        // Store mesh root default position and rotation
        if (meshRoot != null)
        {
            meshRootDefaultLocalPos = meshRoot.localPosition;
            meshRootDefaultLocalRot = meshRoot.localRotation;
        }
    }

    void Update()
    {
        // Clamp very small forwardInput to zero to avoid accidental movement on start
        float clampedForwardInput = Mathf.Abs(forwardInput) < 0.01f ? 0f : forwardInput;
        float speed = IsFreeRunning ? Mathf.Max(0f, clampedForwardInput) * forwardSpeed : forwardSpeed;

        // Apply slide boost if sliding
        if (isSliding)
            speed += slideBoost;

        // Calculate strafe speed as a multiple of forward speed
        float currentSideSpeed = forwardSpeed * strafeSpeedMultiplier;

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

        // Slide logic
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                // Restore collider, keeping the bottom at the same Y
                controller.height = originalColliderHeight;
                Vector3 center = controller.center;
                center.y = originalColliderCenterY;
                controller.center = center;

                // Restore mesh position and rotation
                if (meshRoot != null)
                {
                    meshRoot.localPosition = meshRootDefaultLocalPos;
                    meshRoot.localRotation = meshRootDefaultLocalRot;
                }

                isSliding = false;
            }
        }

        // Calculate movement vector in the new forward direction
        Vector3 right = Vector3.Cross(Vector3.up, moveDirection); // right relative to current forward
        Vector3 moveVector = (moveDirection * speed) + (right * horizontalInput * currentSideSpeed) + (Vector3.up * verticalVelocity);

        // Set animation speed based on horizontal movement magnitude
        Vector3 horizontalMove = moveVector;
        horizontalMove.y = 0f;
        float animSpeed = horizontalMove.magnitude;
        animator.SetFloat("Speed", animSpeed);

        // Set animator playback speed based on forwardSpeed (not actual movement)
        bool isRunning = speed > 0.1f && isGrounded;
        if (isRunning)
        {
            float normalizedSpeed = runAnimationAuthoredSpeed > 0f ? Mathf.Abs(forwardSpeed / runAnimationAuthoredSpeed) : 1f;
            animator.speed = normalizedSpeed;
        }
        else
        {
            animator.speed = 1f;
        }

        controller.Move(moveVector * Time.deltaTime);
    }

    void LateUpdate()
    {
        // Force the meshRoot rotation during slide, after Animator runs
        if (isSliding && meshRoot != null)
        {
            meshRoot.localRotation = meshRootDefaultLocalRot * Quaternion.Euler(0, 45f, 0);
        }
    }

    private void StartSlide()
    {
        if (isSliding || !controller.isGrounded)
            return;

        isSliding = true;
        slideTimer = slideDuration;

        // Set collider to slide values, keeping the bottom at the same Y
        controller.height = slideColliderHeight;
        Vector3 center = controller.center;
        center.y = originalColliderCenterY - (originalColliderHeight - slideColliderHeight) / 2f;
        controller.center = center;

        // Move mesh down to match collider
        if (meshRoot != null)
        {
            float centerOffset = (originalColliderCenterY - (originalColliderHeight - slideColliderHeight) / 2f) - originalColliderCenterY;
            Vector3 pos = meshRootDefaultLocalPos;
            pos.y += centerOffset;
            meshRoot.localPosition = pos;
            // Rotation is handled in LateUpdate
        }

        // Optionally trigger a slide animation
        if (animator != null)
            animator.SetTrigger("Slide");
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
