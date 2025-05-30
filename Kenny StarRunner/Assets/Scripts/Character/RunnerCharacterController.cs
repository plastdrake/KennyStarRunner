using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Store; // For Item.ItemType

public class RunnerCharacterController : MonoBehaviour
{
    public float forwardSpeed = 16f;
    public float strafeSpeedMultiplier = 0.6f;
    public float jumpForce = 200f;
    public float doubleJumpForce = 50f;
    public float gravity = 15f;
    [SerializeField] private float normalGravity = 15f;
    [SerializeField] private float glideGravity = 8f;

    public float turnSpeed = 10f;
    public float freeRunTurnSpeed = 120f;

    public float runAnimationAuthoredSpeed = 8f;
    public bool IsFreeRunning = false;

    public float slideBoost = 4f;
    public float slideDuration = 0.7f;
    public float slideColliderHeight = 1f;

    [SerializeField] private Transform meshRoot;
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
    public Vector3 camFrontLocalPos = new Vector3(0, 2, 3);
    public Vector3 camFrontLocalEuler = new Vector3(10, 180, 0);

    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private float forwardInput = 0f;
    private bool cameraReverseHeld = false;
    private PlayerSounds playerSounds;

    // Falling too long death logic
    [SerializeField] private float maxAirTime = 6.5f; // seconds before death
    private float airTimer = 0f;
    private bool hasFallenToDeath = false;
    [SerializeField] private Health health;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerSounds = GetComponentInChildren<PlayerSounds>();
        inputActions = new InputSystem_Actions();
        moveInput = Vector2.zero;
        forwardInput = 0f;

        if (health == null)
            health = GetComponent<Health>();

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

        // Lock and hide the cursor when the character is enabled
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();

        // Unlock and show the cursor when the character is disabled
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Start()
    {
        moveDirection = transform.forward;
        targetRotation = transform.rotation;

        camTransform = GetComponentInChildren<Camera>().transform;
        camDefaultLocalPos = camTransform.localPosition;
        camDefaultLocalRot = camTransform.localRotation;

        // Ensure animator starts in idle
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsInAir", false);
            animator.SetBool("IsSliding", false);
            animator.speed = 1f;
        }

        // Store original collider values
        if (controller != null)
        {
            originalColliderHeight = controller.height;
            originalColliderCenterY = controller.center.y;
        }

        // Store mesh root default position and rotation
        if (meshRoot != null)
        {
            meshRootDefaultLocalPos = meshRoot.localPosition;
            meshRootDefaultLocalRot = meshRoot.localRotation;
        }
    }

    void Update()
    {
        // --- GLIDE HAT GRAVITY LOGIC ---
        bool hasGlide = false;
        if (GameManager.Instance != null && GameManager.Instance.EquippedPowerupType.HasValue)
        {
            hasGlide = GameManager.Instance.EquippedPowerupType.Value == Item.ItemType.Glide;
        }
        gravity = hasGlide ? glideGravity : normalGravity;
        // --- END GLIDE HAT LOGIC ---

        // Allow the player to unlock the cursor with Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        float clampedForwardInput = Mathf.Abs(forwardInput) < 0.01f ? 0f : forwardInput;
        float speed;
        float horizontalInput = moveInput.x;

        float moveX = 0f;
        float moveY = 0f;

        if (IsFreeRunning)
        {
            speed = clampedForwardInput * forwardSpeed;

            if (clampedForwardInput > 0.01f)
                moveY = 1f;
            else if (clampedForwardInput < -0.01f)
                moveY = -1f;
            else
                moveY = 0f;
            moveX = 0f;

            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                float turnAmount = horizontalInput * freeRunTurnSpeed * Time.deltaTime;
                moveDirection = Quaternion.Euler(0, turnAmount, 0) * moveDirection;
                moveDirection.Normalize();
                targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            }
            if (animator != null)
                animator.SetFloat("Strafe", 0f);
        }
        else
        {
            speed = forwardSpeed;

            moveY = 1f;
            if (horizontalInput > 0.01f)
                moveX = 1f;
            else if (horizontalInput < -0.01f)
                moveX = -1f;
            else
                moveX = 0f;

            if (animator != null)
                animator.SetFloat("Strafe", horizontalInput);
        }

        if (animator != null)
        {
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
        }

        if (isSliding)
            speed += slideBoost;

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

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        bool isGrounded = controller != null && controller.isGrounded;

        if (isGrounded)
        {
            if (animator != null)
                animator.SetBool("IsInAir", false);
            canDoubleJump = true;

            if (jumpRequest)
            {
                verticalVelocity = jumpForce;
                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                    animator.SetBool("IsInAir", true);
                }
                jumpRequest = false;
            }
        }
        else
        {
            if (animator != null)
                animator.SetBool("IsInAir", true);
            verticalVelocity -= gravity * Time.deltaTime;

            if (doubleJumpRequest && canDoubleJump)
            {
                verticalVelocity = doubleJumpForce;
                if (animator != null)
                    animator.SetTrigger("DoubleJump");
                canDoubleJump = false;
                doubleJumpRequest = false;
            }
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                if (controller != null)
                {
                    controller.height = originalColliderHeight;
                    Vector3 center = controller.center;
                    center.y = originalColliderCenterY;
                    controller.center = center;
                }

                if (meshRoot != null)
                {
                    meshRoot.localPosition = meshRootDefaultLocalPos;
                }

                isSliding = false;
                if (animator != null)
                    animator.SetBool("IsSliding", false);
            }
        }

        Vector3 moveVector;
        if (IsFreeRunning)
        {
            moveVector = (moveDirection * speed) + (Vector3.up * verticalVelocity);
        }
        else
        {
            float currentSideSpeed = forwardSpeed * strafeSpeedMultiplier;
            Vector3 right = Vector3.Cross(Vector3.up, moveDirection);
            moveVector = (moveDirection * speed) + (right * horizontalInput * currentSideSpeed) + (Vector3.up * verticalVelocity);
        }

        Vector3 horizontalMove = moveVector;
        horizontalMove.y = 0f;
        float animSpeed = horizontalMove.magnitude;
        if (animator != null)
            animator.SetFloat("Speed", animSpeed);

        bool isRunning = Mathf.Abs(speed) > 0.1f && isGrounded;
        if (animator != null)
        {
            if (isRunning)
            {
                float normalizedSpeed = runAnimationAuthoredSpeed > 0f ? Mathf.Abs(forwardSpeed / runAnimationAuthoredSpeed) : 1f;
                animator.speed = normalizedSpeed;
            }
            else
            {
                animator.speed = 1f;
            }
        }

        if (controller != null)
            controller.Move(moveVector * Time.deltaTime);

        // "Falling too long" death logic
        if (controller != null && !controller.isGrounded)
        {
            airTimer += Time.deltaTime;
            if (!hasFallenToDeath && airTimer >= maxAirTime)
            {
                hasFallenToDeath = true;
                if (health != null)
                    health.TakeDamage(9999, gameObject); // Triggers death
            }
        }
        else
        {
            airTimer = 0f;
            hasFallenToDeath = false;
        }
    }

    void LateUpdate()
    {
        if (meshRoot != null)
        {
            if (isSliding)
                meshRoot.localRotation = meshRootDefaultLocalRot * Quaternion.Euler(0, 45f, 0);
            else
                meshRoot.localRotation = meshRootDefaultLocalRot;
        }
    }

    private void StartSlide()
    {
        if (isSliding || controller == null || !controller.isGrounded)
            return;

        if (IsFreeRunning)
        {
            if (forwardInput <= 0.01f)
                return;
        }

        isSliding = true;
        slideTimer = slideDuration;

        if (controller != null)
        {
            controller.height = slideColliderHeight;
            Vector3 center = controller.center;
            center.y = originalColliderCenterY - (originalColliderHeight - slideColliderHeight) / 2f;
            controller.center = center;
        }

        if (meshRoot != null)
        {
            float centerOffset = (originalColliderCenterY - (originalColliderHeight - slideColliderHeight) / 2f) - originalColliderCenterY;
            Vector3 pos = meshRootDefaultLocalPos;
            pos.y += centerOffset;
            meshRoot.localPosition = pos;
        }

        if (animator != null)
        {
            animator.SetBool("IsSliding", true);
        }
    }

    private void OnJump()
    {
        if (controller != null && controller.isGrounded)
        {
            jumpRequest = true;
            if (playerSounds != null && Random.value < 0.1f)
                playerSounds.JumpSound();
        }
        else if (canDoubleJump)
            doubleJumpRequest = true;
    }

    private void OnTurnLeft()
    {
        if (!IsFreeRunning)
        {
            moveDirection = Quaternion.Euler(0, -90, 0) * moveDirection;
            targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
    }

    private void OnTurnRight()
    {
        if (!IsFreeRunning)
        {
            moveDirection = Quaternion.Euler(0, 90, 0) * moveDirection;
            targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
    }
}
