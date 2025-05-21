using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 7.0f;
    public float rotationSpeed = 90f; // Instant, men kan bli smooth

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Spring alltid framåt i den riktning spelaren är vänd
        transform.position += transform.forward * speed * Time.deltaTime;

        // Sväng höger (D) = rotera +90 grader runt Y
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(0, 90f, 0);
        }

        // Sväng vänster (A) = rotera -90 grader runt Y
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(0, -90f, 0);
        }

        // Hoppa
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
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
