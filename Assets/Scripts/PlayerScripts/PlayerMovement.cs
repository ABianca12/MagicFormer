using Unity.Hierarchy;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float crouchMoveSpeed = 2f;
    [SerializeField] float handstandMoveSpeed = 2f;
    [SerializeField] float airMultiplier = 0.4f;
    private float movementMultiplier = 10f;

    [Header("Camera Effects")]
    [SerializeField] Camera cam;

    [Header("Jumping")]
    public float jumpForce = 50f;
    public float standingToHandstandJumpForce;
    public float superHandstandJumpForce;
    public float jumpRate = 15f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode crouchKey = KeyCode.S;

    [Header("Gravity and Air Control")]
    [SerializeField] float groundDrag = 2f;
    [SerializeField] float airDrag = 0.4f;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    public bool isGrounded { get; private set; }

    [HideInInspector] public bool isMoving;

    private Vector3 moveDirection;
    private Vector2 movement;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isMoving = (moveDirection != Vector3.zero);

        HandleInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKey(jumpKey) && isGrounded)
        {
            HandleJump();
        }
        
        if (Input.GetKeyDown(crouchKey))
        {
            
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        rotatePlayer();
    }

    void HandleInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        moveDirection = gameObject.transform.forward * -movement.y + gameObject.transform.right * -movement.x;
    }

    void HandleJump()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void ControlSpeed()
    {
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void ControlDrag()
    {
        rb.linearDamping = isGrounded ? groundDrag : airDrag;
    }

    void MovePlayer()
    {
        float multiplier = isGrounded ? movementMultiplier : airMultiplier * movementMultiplier;
        rb.AddForce(multiplier * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
    }

    void rotatePlayer()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint col in collision.contacts)
        {
            if (col.point.y <= transform.position.y + 0.1 && !isGrounded)
            {
                isGrounded = true;
            }
        }
    }
}
