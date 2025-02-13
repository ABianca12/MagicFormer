using System;
using UnityEngine;

namespace TarodevController
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private MovementVariables moveVars;
        private Rigidbody2D rb;
        private CapsuleCollider2D capCollider;
        private FrameInput frameInput;
        private Vector2 velocity;
        private bool startInColliders;

        #region Interface

        public Vector2 FrameInput => frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            capCollider = GetComponent<CapsuleCollider2D>();

            startInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            frameInput = new FrameInput
            {
                // If Mathf.Abs(frameInput.move.x) is less than HorizontalDeadZoneThreshold, then
                // frameInput.move.x = 0, if Mathf.Abs(frameInput.move.x) is not less than
                // HorizontalDeadZoneThreshold frameInput.move.x is equal to the sign of its value
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (moveVars.SnapInput)
            {
                frameInput.Move.x = Mathf.Abs(frameInput.Move.x) < moveVars.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.x);
                frameInput.Move.y = Mathf.Abs(frameInput.Move.y) < moveVars.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.y);
            }

            if (frameInput.JumpDown)
            {
                hasJump = true;
                timeJumpWasPressed = _time;
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();

            ApplyMovement();
        }

        #region Collisions

        private float frameLeftGround = float.MinValue;
        private bool grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.down, moveVars.GrounderDistance, ~moveVars.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.up, moveVars.GrounderDistance, ~moveVars.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) velocity.y = Mathf.Min(0, velocity.y);

            // Landed on the Ground
            if (!grounded && groundHit)
            {
                grounded = true;
                coyoteUsable = true;
                bufferedJumpUsable = true;
                endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(velocity.y));
            }
            // Left the Ground
            else if (grounded && !groundHit)
            {
                grounded = false;
                frameLeftGround = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = startInColliders;
        }

        #endregion

        #region Jumping

        private bool hasJump;
        private bool bufferedJumpUsable;
        private bool endedJumpEarly;
        private bool coyoteUsable;
        private float timeJumpWasPressed;

        private bool HasBufferedJump => bufferedJumpUsable && _time < timeJumpWasPressed + moveVars.JumpBuffer;
        private bool CanUseCoyote => coyoteUsable && !grounded && _time < frameLeftGround + moveVars.CoyoteTime;

        private void HandleJump()
        {
            if (!endedJumpEarly && !grounded && !frameInput.JumpHeld && rb.linearVelocity.y > 0) endedJumpEarly = true;

            if (!hasJump && !HasBufferedJump) return;

            if (grounded || CanUseCoyote) ExecuteJump();

            hasJump = false;
        }

        private void ExecuteJump()
        {
            endedJumpEarly = false;
            timeJumpWasPressed = 0;
            bufferedJumpUsable = false;
            coyoteUsable = false;
            velocity.y = moveVars.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (frameInput.Move.x == 0)
            {
                var deceleration = grounded ? moveVars.GroundDeceleration : moveVars.AirDeceleration;
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, frameInput.Move.x * moveVars.MaxSpeed, moveVars.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (grounded && velocity.y <= 0f)
            {
                velocity.y = moveVars.GroundingForce;
            }
            else
            {
                var inAirGravity = moveVars.FallAcceleration;
                if (endedJumpEarly && velocity.y > 0) inAirGravity *= moveVars.JumpEndEarlyGravityModifier;
                velocity.y = Mathf.MoveTowards(velocity.y, -moveVars.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => rb.linearVelocity = velocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (moveVars == null) Debug.LogWarning("Please assign a MovementVaribles asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}
