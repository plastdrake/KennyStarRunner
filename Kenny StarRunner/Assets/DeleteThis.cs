using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class DeleteThis : MonoBehaviour
{
    public float moveSpeed = 2f;   

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity = 0f;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

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
    }

    void Update()
    {

        transform.Rotate(Vector3.up, moveInput.x * 200f * Time.deltaTime);

        Vector3 horizontalMoveDirection = transform.forward * moveInput.y;

        Vector3 moveVector = horizontalMoveDirection.normalized * moveSpeed * Mathf.Abs(moveInput.y);

        if (Mathf.Abs(moveInput.y) < 0.01f)
        {
            moveVector.x = 0;
            moveVector.z = 0;
        }

        moveVector.y = verticalVelocity;
        controller.Move(moveVector * Time.deltaTime);


        float animSpeed = Mathf.Abs(moveInput.y) * moveSpeed;
        animator.SetFloat("Speed", animSpeed);
    }
}