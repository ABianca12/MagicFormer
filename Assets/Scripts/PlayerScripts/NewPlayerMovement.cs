using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]

public class NewPlayerMovement : MonoBehaviour
{
    public struct FrameInput
    {
        public bool jumpPressed;
        public bool jumpHeld;
        public Vector2 move;
    }

    [SerializeField] private MovementVaribles movementVaribles;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private FrameInput frameInput;
    private Vector2 velocity;
    // Do raycasts detect the colliders they come from
    private bool startInColliders;

    private float time;

    #region Interface
    public Vector2 frameInputV2 => frameInput.move;
    public event Action<bool, float> groundedChange;
    public event Action jump;
    #endregion

    #region Jumping
    private bool hasJump;
    private bool canUsejumpBuffering;
    private bool jumpEndedEarly;
    private bool isCoyoteUsable;
    private float timeJumpPressed;

    private bool hasBufferedJump => canUsejumpBuffering && time <
        timeJumpPressed + movementVaribles.JumpBuffer;
    private bool canUseCoyoteTime => isCoyoteUsable && !grounded && time <
        frameLeftGround + movementVaribles.CoyoteTime;

    private void HandleJump()
    {
        if (!jumpEndedEarly && !grounded && !frameInput.jumpHeld && rb.linearVelocity.y > 0)
        {
            jumpEndedEarly = true;
        }

        if (!hasJump && !hasBufferedJump)
        {
            return;
        }

        if (grounded || canUseCoyoteTime)
        {
            ExecuteJump();
        }

        hasJump = false;
    }

    private void ExecuteJump()
    {
        
    }

    #endregion

    #region Collisions
    private float frameLeftGround = float.MinValue;
    private bool grounded;

    private void CheckCollisions()
    {
        startInColliders = false;

        bool groundHit = Physics.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size,
            capsuleCollider.radius, Vector3.down, movementVaribles.GrounderDistance,
            movementVaribles.PlayerLayer);
        bool ceilingHit = Physics.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size,
            capsuleCollider.radius, Vector3.up, movementVaribles.GrounderDistance,
            movementVaribles.PlayerLayer);

        // Hit a ceiling
        if (ceilingHit)
        {
            velocity.y = Mathf.Min(0, velocity.y);
        }

        // Landed on the ground
        if (!grounded && groundHit)
        {
            grounded = true;
            isCoyoteUsable = true;
            canUsejumpBuffering = true;
            jumpEndedEarly = false;
            groundedChange?.Invoke(true, Mathf.Abs(velocity.y));
        }
        // Left Ground
        else if (grounded && !groundHit)
        {
            grounded = false;
            frameLeftGround = time;
            groundedChange?.Invoke(false, 0);
        }

    }

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        startInColliders = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime;
        GatherInput();
    }

    private void GatherInput()
    {
        frameInput = new FrameInput
        {
            jumpPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(movementVaribles.jump),
            jumpHeld = Input.GetButton("Jump") || Input.GetKeyDown(movementVaribles.jump),
            move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
        };

        if (movementVaribles.SnapInput)
        {
            // If Mathf.Abs(frameInput.move.x) is less than HorizontalDeadZoneThreshold, then
            // frameInput.move.x = 0, if Mathf.Abs(frameInput.move.x) is not less than
            // HorizontalDeadZoneThreshold frameInput.move.x is equal to the sign of its value
            frameInput.move.x = Mathf.Abs(frameInput.move.x) <
                movementVaribles.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.move.x);
            frameInput.move.y = Mathf.Abs(frameInput.move.y) <
                movementVaribles.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.move.y);
        }

        if (frameInput.jumpPressed)
        {
            hasJump = true;
            timeJumpPressed = time;
        }
    }

    private void FixedUpdate()
    {
        CheckCollisions();
    }
}
