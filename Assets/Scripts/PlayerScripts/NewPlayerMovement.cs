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
        private bool startInColliders = false;

        #region Interface

        public Vector2 FrameInput => frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        public enum PlayerState
        {
            None = 0,
            Handstand,
            SingleRope,
            DoubleRope,
            HorizontalBar,
            Crouching,
            Dead,
            Carrying,
            Falling,
        }

        private PlayerState state = PlayerState.None;

        public PlayerState GetPlayerState()
        {
            return state;
        }

        public enum PlayerDirection
        {
            None,
            Left,
            Right
        }

        private PlayerDirection facing = PlayerDirection.Right;
        
        public PlayerDirection getFaceDirection()
        {
            return facing;
        }

        #endregion

        private float time;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            capCollider = GetComponent<CapsuleCollider2D>();
            startInColliders = Physics2D.queriesStartInColliders; 
        }

        private void Update()
        {
            time += Time.deltaTime;
            GatherInput();
            HandlePickUp();
        }

        private void GatherInput()
        {
            frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(moveVars.jump),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(moveVars.jump),
                PickUpDown = Input.GetKeyDown(moveVars.pickUp),
                PickUpHeld = Input.GetKey(moveVars.pickUp),

                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (moveVars.SnapInput)
            {
                // If Mathf.Abs(frameInput.move.x) is less than HorizontalDeadZoneThreshold, then
                // frameInput.move.x = 0, if Mathf.Abs(frameInput.move.x) is not less than
                // HorizontalDeadZoneThreshold frameInput.move.x is equal to the sign of its value
                frameInput.Move.x = Mathf.Abs(frameInput.Move.x) < moveVars.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.x);
                frameInput.Move.y = Mathf.Abs(frameInput.Move.y) < moveVars.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.y);
            }

            if (frameInput.JumpDown)
            {
                hasJump = true;
                timeJumpWasPressed = time;
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDown();
            HandleUp();
            HandleDirection();

            if (!(state == PlayerState.SingleRope || state == PlayerState.DoubleRope))
            {
                HandleGravity();
            }

            ApplyMovement();
        }

        #region Collisions

        private float frameLeftGround = float.MinValue;
        private bool grounded;
        private static bool inRangeOfRope;
        private bool isOnTopOfPickup;
        private bool ceilingHit = false;
        private RaycastHit2D hit;
        private GameObject ObjectOnTopOf;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.down, moveVars.GrounderDistance, ~moveVars.PlayerLayer);
            
            hit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.down, moveVars.GrounderDistance, ~moveVars.PlayerLayer);

            if (hit)
            {
                if (hit.transform.tag == "PickUp")
                {
                    isOnTopOfPickup = true;
                    ObjectOnTopOf = hit.transform.gameObject;
                }
                else
                {
                    isOnTopOfPickup = false;
                    ObjectOnTopOf = null;
                }
            }

            if (state == PlayerState.Crouching)
            {
                ceilingHit = Physics2D.CapsuleCast(capCollider.bounds.center, new Vector2(capCollider.size.x, capCollider.size.y * 10 ), capCollider.direction,
                0, Vector2.up, moveVars.GrounderDistance, ~moveVars.PlayerLayer);
                Debug.DrawRay(transform.position, transform.up * capCollider.size.y, Color.yellow);
            }
            else
            {
                ceilingHit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.up, moveVars.GrounderDistance, ~moveVars.PlayerLayer);
            }

            // Hit a Ceiling
            if (ceilingHit && state == PlayerState.None)
            {
                velocity.y = Mathf.Min(0, velocity.y);
            }

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
                frameLeftGround = time;
                GroundedChanged?.Invoke(false, 0);
            }

            if ((state == PlayerState.SingleRope || state == PlayerState.DoubleRope) && !inRangeOfRope)
            {
                state = PlayerState.None;
            }

            Physics2D.queriesStartInColliders = startInColliders;
        }

        public static void setInRangeOfRope(bool newValue)
        {
            inRangeOfRope = newValue;
        }

        #endregion

        #region Jumping

        private bool hasJump;
        private bool bufferedJumpUsable;
        private bool endedJumpEarly;
        private bool coyoteUsable;
        private float timeJumpWasPressed;

        private bool HasBufferedJump => bufferedJumpUsable && time < timeJumpWasPressed + moveVars.JumpBuffer;
        private bool CanUseCoyote => coyoteUsable && !grounded && time < frameLeftGround + moveVars.CoyoteTime;

        private void HandleJump()
        {
            if (!endedJumpEarly && !grounded && !frameInput.JumpHeld && rb.linearVelocity.y > 0)
            {
                endedJumpEarly = true;
            }

            if (!hasJump && !HasBufferedJump)
            {
                return;
            }

            if (grounded || CanUseCoyote)
            {
                ExecuteJump();
            }

            hasJump = false;
        }

        private void ExecuteJump()
        {
            endedJumpEarly = false;
            timeJumpWasPressed = 0;
            bufferedJumpUsable = false;
            coyoteUsable = false;
            switch (state)
            {
                case PlayerState.Crouching:
                    velocity.y = moveVars.HandStandTransitionJumpPower;
                    state = PlayerState.Handstand;
                    RotatePlayer();
                    break;
                case PlayerState.Handstand:
                    velocity.y = moveVars.HandStandJumpPower;
                    state = PlayerState.None;
                    RotatePlayer();
                    break;
                default:
                    velocity.y = moveVars.JumpPower;
                    break;
            }
            Jumped?.Invoke();
        }

        #endregion

        #region Down Input

        private float timeDownWasPressed;

        private void HandleDown()
        {
            switch (state)
            {
                case PlayerState.None:
                    if (frameInput.Move.y == -1)
                    {
                        transform.localScale = new Vector3(1, 0.5f, 1);
                        state = PlayerState.Crouching;
                    }
                    break;
                case PlayerState.Crouching:
                    if (frameInput.Move.y == 0 && !ceilingHit)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        state = PlayerState.None;
                    }
                    break;
                case PlayerState.Handstand:
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case PlayerState.SingleRope:
                    if (frameInput.Move.y == -1)
                    {
                        velocity.y = -moveVars.MaxDownwardsSingleRopeSpeed;
                    }
                    break;
                case PlayerState.DoubleRope:
                    if (frameInput.Move.y == -1)
                    {
                        velocity.y = -moveVars.MaxDownwardsDoubleRopeSpeed;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Up Input

        private float timeUpWasPressed;

        private void HandleUp()
        {
            switch (state)
            {
                case PlayerState.None:
                    if (inRangeOfRope)
                    {
                        if (frameInput.Move.y == 1)
                        {
                            velocity.x = 0;
                            velocity.y = 0;
                            state = PlayerState.SingleRope;
                        }
                    }
                    break;
                case PlayerState.SingleRope:
                    if (frameInput.Move.y == 1)
                    {
                        velocity.y = moveVars.MaxUpwardsSingleRopeSpeed;
                    }
                    else if (frameInput.Move.y == 0)
                    {
                        velocity.y = 0;
                    }
                    break;
                case PlayerState.DoubleRope:
                    if (frameInput.Move.y == 1)
                    {
                        velocity.y = moveVars.MaxUpwardsDoubleRopeSpeed;
                    }
                    else if (frameInput.Move.y == 0)
                    {
                        velocity.y = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Pickups

        private GameObject ObjectBeingHeld;

        private void HandlePickUp()
        {
            switch(state)
            {
                case PlayerState.None:
                    if (frameInput.PickUpDown && isOnTopOfPickup)
                    {
                        ObjectBeingHeld = ObjectOnTopOf;
                        state = PlayerState.Carrying;
                    }
                    break;
                case PlayerState.Carrying:
                    if (frameInput.PickUpDown)
                    {
                        state = PlayerState.None;
                        ObjectBeingHeld = null;
                        if (frameInput.Move.y == 1)
                        {
                            PickUpBehvaior.ThrowPickUp(new Vector2(0,
                                moveVars.ThrowingStrength + velocity.y));
                            Debug.Log("Object thrown");
                        }
                        else if (frameInput.Move.y == -1)
                        {
                            PickUpBehvaior.ThrowPickUp(new Vector2(0, 0));
                            Debug.Log("Object thrown");
                        }
                        else if (frameInput.Move.x == -1)
                        {
                            PickUpBehvaior.ThrowPickUp(new Vector2(velocity.x + -moveVars.ThrowingStrength,
                                moveVars.ThrowingStrength));
                            Debug.Log("Object thrown");
                        }
                        else if (frameInput.Move.x == 1)
                        {
                            PickUpBehvaior.ThrowPickUp(new Vector2(velocity.x + moveVars.ThrowingStrength,
                                moveVars.ThrowingStrength));
                            Debug.Log("Object thrown");
                        }
                        else
                        {
                            switch (facing)
                            {
                                case PlayerDirection.Left:
                                    PickUpBehvaior.ThrowPickUp(new Vector2(-moveVars.ThrowingStrength,
                                moveVars.ThrowingStrength / 2));
                                    Debug.Log("Object thrown");
                                    break;
                                case PlayerDirection.Right:
                                    PickUpBehvaior.ThrowPickUp(new Vector2(moveVars.ThrowingStrength,
                                moveVars.ThrowingStrength / 2));
                                    Debug.Log("Object thrown");
                                    break;
                            }
                        }
                    }
                    break;
            }
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
                switch(state)
                {
                    case PlayerState.Crouching:
                        velocity.x = Mathf.MoveTowards(velocity.x,
                            frameInput.Move.x * moveVars.MaxCrouchSpeed,
                            moveVars.CrouchAcceleration * Time.fixedDeltaTime);
                        RotatePlayer();
                        break;
                    case PlayerState.Handstand:
                        velocity.x = Mathf.MoveTowards(velocity.x,
                            frameInput.Move.x * moveVars.MaxHandStandSpeed,
                            moveVars.HandstandAcceleration * Time.fixedDeltaTime);
                        RotatePlayer();
                        break;
                    case PlayerState.SingleRope:
                        RotatePlayer();
                        break;
                    default:
                        velocity.x = Mathf.MoveTowards(velocity.x, frameInput.Move.x * moveVars.MaxSpeed,
                            moveVars.Acceleration * Time.fixedDeltaTime);
                        RotatePlayer();
                        break;
                }
            }
        }

        private void RotatePlayer()
        {
            switch(state)
            {
                default:
                    if (frameInput.Move.x == -1)
                    {
                        facing = PlayerDirection.Left;
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (frameInput.Move.x == 1)
                    {
                        facing = PlayerDirection.Right;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        if (facing == PlayerDirection.Left)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        else if (facing == PlayerDirection.Right)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                    }
                    break;
                case PlayerState.Handstand:
                    if (frameInput.Move.x == -1)
                    {
                        facing = PlayerDirection.Left;
                        transform.rotation = Quaternion.Euler(0, 180, 180);
                    }
                    else if (frameInput.Move.x == 1)
                    {
                        facing = PlayerDirection.Right;
                        transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else
                    {
                        if (facing == PlayerDirection.Left)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 180);
                        }
                        else if (facing == PlayerDirection.Right)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 180);
                        }
                    }
                    break;
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
        public bool PickUpDown; 
        public bool PickUpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}
