using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool moveLeft, moveRight, moveForward, moveBack, jump;
    public float speed = 7.0f;
    public float jumpForce = 5.0f;
    private Rigidbody rb;
    private bool isGrounded = true;
    void Start()
    {
        //myCharacterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        moveLeft = Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.D);
        moveForward = Input.GetKey(KeyCode.W);
        moveBack = Input.GetKey(KeyCode.S);
        jump = Input.GetKeyDown(KeyCode.Space);

        Vector3 movement = Vector3.zero;

        if (moveForward)
            movement += transform.forward;
        if (moveBack)
            movement -= transform.forward;
        if (moveRight)
            movement += transform.right;
        if (moveLeft)
            movement -= transform.right;

        transform.position += movement.normalized * speed * Time.deltaTime;

        if (jump && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
