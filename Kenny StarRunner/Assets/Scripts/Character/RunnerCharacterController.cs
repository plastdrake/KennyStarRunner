using UnityEngine;

public class RunnerCharacterController : MonoBehaviour
{
    public float forwardSpeed = 8f;
    public float sideSpeed = 5f;
    public float jumpForce = 200f;
    public float doubleJumpForce = 50f;
    public float gravity = 15f;

    private float verticalVelocity;
    private bool canDoubleJump = false;
    private bool jumpRequest = false;
    private bool doubleJumpRequest = false;

    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Always move forward
        float speed = forwardSpeed;
        animator.SetFloat("Speed", speed);

        float horizontalInput = Input.GetAxis("Horizontal"); // -1 (left), 0 (center), 1 (right)
        animator.SetFloat("Strafe", horizontalInput);

        // Input handling
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

        // Move the character
        Vector3 moveVector = new Vector3(horizontalInput * sideSpeed, verticalVelocity, speed);
        controller.Move(moveVector * Time.deltaTime);
    }
}
