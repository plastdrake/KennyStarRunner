using UnityEngine;

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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        moveDirection = Vector3.forward;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Always move forward
        float speed = forwardSpeed;
        animator.SetFloat("Speed", speed);

        // Free side-to-side movement (relative to current forward)
        float horizontalInput = Input.GetAxis("Horizontal"); // -1 (left), 0 (center), 1 (right)
        animator.SetFloat("Strafe", horizontalInput);

        // Handle 90° turns
        if (Input.GetKeyDown(KeyCode.Q))
        {
            moveDirection = Quaternion.Euler(0, -90, 0) * moveDirection;
            targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            moveDirection = Quaternion.Euler(0, 90, 0) * moveDirection;
            targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }

        // Smoothly rotate character (and camera, if childed) to new direction
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // Input handling for jump/double jump
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (controller.isGrounded)
                jumpRequest = true;
            else if (canDoubleJump)
                doubleJumpRequest = true;
        }

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
}
