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
        private Renderer MainPlayerRenderer;
        public GameObject Hat;
        private Renderer HatRenderer;
        private FrameInput frameInput;
        public Vector2 velocity;
        private bool startInColliders = false;

        public GameObject CrouchingPlayer;

        #region Interface

        public Vector2 FrameInput => frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;
        private Climbable climbable;
        private Climbable LeftClimbable;
        private Climbable RightClimbable;

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

        [SerializeField]
        private PlayerState state = PlayerState.None;

        public PlayerState GetPlayerState()
        {
            return state;
        }

        public enum PlayerDirection
        {
            None,
            Left,
            Right,
            Straight
        }

        [SerializeField]
        private PlayerDirection facing = PlayerDirection.Right;
        
        public PlayerDirection getFaceDirection()
        {
            return facing;
        }

        public enum PlayerLooking
        {
            None,
            Up,
            Down
        }

        #endregion

        private float time;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            capCollider = GetComponent<CapsuleCollider2D>();
            MainPlayerRenderer = GetComponent<Renderer>();

            HatRenderer = Hat.GetComponent<Renderer>();

            CrouchingPlayer.gameObject.SetActive(false);

            startInColliders = Physics2D.queriesStartInColliders; 
        }

        private void Update()
        {
            time += Time.deltaTime;

            CheckCollisions();
            GatherInput();
            HandlePickUp();
            HandleJump();

            if (state == PlayerState.SingleRope)
            {
                if (frameInput.LeftDown)
                {
                    if (facing == PlayerDirection.Right && LeftRopeHit && LeftClimbable != climbable)
                    {
                        state = PlayerState.DoubleRope;
                        SnapToMiddle();
                        RotatePlayer();
                    }
                    else
                    {
                        SnapToRope(PlayerDirection.Right);
                        RotatePlayer();
                    }
                }
                else if (frameInput.RightDown)
                {
                    if (facing == PlayerDirection.Left && RightRopeHit && RightClimbable != climbable)
                    {
                        state = PlayerState.DoubleRope;
                        SnapToMiddle();
                        RotatePlayer();
                    }
                    else
                    {
                        SnapToRope(PlayerDirection.Left);
                        RotatePlayer();
                    }
                }
            }
            else if (state == PlayerState.DoubleRope)
            {
                if (frameInput.LeftDown)
                {
                    if (facing == PlayerDirection.Straight && LeftRopeHit)
                    {
                        state = PlayerState.SingleRope;
                        facing = PlayerDirection.Left;
                        frameInput.LeftDown = false;
                        RotatePlayer();
                        SnapToLeftRope();
                    }
                }
                else if (frameInput.RightDown)
                {
                    if (facing == PlayerDirection.Straight && RightRopeHit)
                    {
                        state = PlayerState.SingleRope;
                        facing = PlayerDirection.Right;
                        frameInput.RightDown = false;
                        RotatePlayer();
                        SnapToRightRope();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                grounded = true;
                coyoteUsable = true;
                bufferedJumpUsable = true;
                endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(velocity.y));
            }
        }

        private void GatherInput()
        {
            frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(moveVars.jump),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(moveVars.jump),
                PickUpDown = Input.GetKeyDown(moveVars.pickUp),
                PickUpHeld = Input.GetKey(moveVars.pickUp),
                UpDown = Input.GetKeyDown(moveVars.up),
                UpHeld = Input.GetKey(moveVars.up),
                DownDown = Input.GetKeyDown(moveVars.down),
                DownHeld = Input.GetKey(moveVars.down),
                LeftDown = Input.GetKeyDown(moveVars.left),
                RightDown = Input.GetKeyDown(moveVars.right),

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

            if (frameInput.UpDown)
            {
                timeUpWasPressed = time;
            }
        }

        private void FixedUpdate()
        {
            HandleDown();
            HandleUp();

            if (!(state == PlayerState.HorizontalBar && frameInput.Move.y == 1))
            {
                HandleDirection();
            }

            if (!(state == PlayerState.SingleRope || state == PlayerState.DoubleRope || state == PlayerState.HorizontalBar))
            {
                HandleGravity();
            }

            ApplyMovement();
        }

        #region Collisions

        private float frameLeftGround = float.MinValue;
        private float timeHandstandSetupLanded;
        [SerializeField]
        private bool grounded;
        private bool inRangeOfRope;
        private bool inRangeOfBar;
        private bool isOnTopOfPickup;
        private bool ceilingHit;
        private RaycastHit2D groundHit;
        private RaycastHit2D LeftRopeHit;
        private RaycastHit2D RightRopeHit;
        private GameObject ObjectOnTopOf;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            groundHit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.down, moveVars.GrounderDistance, ~moveVars.PlayerLayer);

            ceilingHit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.up, moveVars.GrounderDistance, ~moveVars.PlayerLayer);

            LeftRopeHit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.left, moveVars.RopeGrabbingRange, moveVars.ClimbableLayer);

            RightRopeHit = Physics2D.CapsuleCast(capCollider.bounds.center, capCollider.size, capCollider.direction,
                0, Vector2.right, moveVars.RopeGrabbingRange, moveVars.ClimbableLayer);

            if (groundHit)
            {
                if (groundHit.transform.tag == "PickUp")
                {
                    isOnTopOfPickup = true;
                    ObjectOnTopOf = groundHit.transform.gameObject;
                }
                else
                {
                    isOnTopOfPickup = false;
                    ObjectOnTopOf = null;
                }
            }

            if (state == PlayerState.Crouching)
            {
                ceilingHit = Physics2D.CapsuleCast(capCollider.bounds.center, new Vector2(capCollider.size.x, capCollider.size.y * 10), capCollider.direction,
                0, Vector2.up, moveVars.GrounderDistance, ~moveVars.PlayerLayer);
                Debug.DrawRay(transform.position, transform.up * capCollider.size.y, Color.yellow);
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

                if (canHandstandJump)
                {
                    timeHandstandSetupLanded = time;
                }

                canCrouch = true;
                
            }
            // Left the Ground
            else if (grounded && !groundHit)
            {
                grounded = false;
                frameLeftGround = time;
                GroundedChanged?.Invoke(false, 0);
                canCrouch = false;
            }
            else if (!grounded && !groundHit)
            {
                if (state == PlayerState.SingleRope || state == PlayerState.DoubleRope || state == PlayerState.HorizontalBar || state == PlayerState.Crouching)
                {
                    grounded = true;
                    coyoteUsable = true;
                    bufferedJumpUsable = true;
                    endedJumpEarly = false;
                    GroundedChanged?.Invoke(true, Mathf.Abs(velocity.y));
                }
            }

            if (state == PlayerState.HorizontalBar && !inRangeOfBar)
            {
                state = PlayerState.None;
            }

            if (state == PlayerState.SingleRope || state == PlayerState.DoubleRope)
            {
                if (transform.position.y < climbable.GetRightTransform().y)
                {
                    state = PlayerState.None;
                    Debug.Log(climbable.GetRightTransform().y);
                    Debug.Log("Player is lower than bottom");
                }

                if (LeftRopeHit)
                {
                    LeftClimbable = LeftRopeHit.transform.GetComponent<Climbable>();
                }

                if (RightRopeHit)
                {
                    RightClimbable = RightRopeHit.transform.GetComponent<Climbable>();
                }

                Debug.DrawRay(transform.position, Vector2.left, Color.blue);
                Debug.DrawRay(transform.position, Vector2.right, Color.red);
            }

            Physics2D.queriesStartInColliders = startInColliders;
        }

        public void setInRangeOfRope(bool newValue)
        {
            inRangeOfRope = newValue;
        }

        public void setInRangeOfBar(bool newValue)
        {
            inRangeOfBar = newValue;
        }

        #endregion

        #region Jumping

        private bool hasJump;
        private bool bufferedJumpUsable;
        private bool endedJumpEarly;
        private bool coyoteUsable;
        private float timeJumpWasPressed;
        private bool canHandstandJump;
        private float timeSinceHandstandSetupLanded;
        private float timeSinceDirectionChange;

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
            timeSinceHandstandSetupLanded = time - timeHandstandSetupLanded;
            timeSinceDirectionChange = time - timeDirectionChanged;

            switch (state)
            {
                case PlayerState.Crouching:
                    velocity.y = moveVars.HandStandTransitionJumpPower;
                    state = PlayerState.Handstand;
                    RotatePlayer();
                    break;
                case PlayerState.Handstand:
                    velocity.y = moveVars.JumpPower;
                    canHandstandJump = true;
                    state = PlayerState.None;
                    RotatePlayer();
                    break;
                case PlayerState.HorizontalBar:
                    if (timeUpHasBeenHeld >= moveVars.MaxBarUpHoldTime)
                    {
                        velocity.y = moveVars.MaxBarJumpPower;
                    }
                    else
                    {
                        velocity.y = moveVars.MinBarJumpPower * timeUpHasBeenHeld;
                    }
                    break;
                case PlayerState.SingleRope:
                    state = PlayerState.None;
                    velocity.y = moveVars.JumpPower;
                    canHandstandJump = false;
                    break;
                case PlayerState.DoubleRope:
                    state = PlayerState.None;
                    velocity.y = moveVars.JumpPower;
                    canHandstandJump = false;
                    break;
                default:
                    if (timeSinceDirectionChange <= moveVars.BackFlipJumpTime)
                    {
                        velocity.y = moveVars.BackFlipJumpPower;
                        canHandstandJump = false;
                        Debug.Log("Backflip");
                    }
                    else if (canHandstandJump && timeSinceHandstandSetupLanded <= moveVars.HandStandJumpTime)
                    {
                        velocity.y = moveVars.HandStandJumpPower;
                        canHandstandJump = false;
                    }
                    else
                    {
                        velocity.y = moveVars.JumpPower;
                        canHandstandJump = false;
                    }
                    break;
            }
            endedJumpEarly = false;
            timeJumpWasPressed = 0;
            bufferedJumpUsable = false;
            coyoteUsable = false;

            Jumped?.Invoke();
        }

        #endregion

        #region Down Input

        private float timeDownWasPressed;
        private bool canCrouch;

        private void HandleDown()
        {
            switch (state)
            {
                case PlayerState.None:
                    if (frameInput.Move.y == -1 && canCrouch)
                    {
                        capCollider.size = new Vector2(capCollider.size.x, 1);
                        MainPlayerRenderer.enabled = false;
                        HatRenderer.enabled = false;
                        CrouchingPlayer.gameObject.SetActive(true);
                        //transform.localScale = new Vector3(1, 0.5f, 1);
                        state = PlayerState.Crouching;
                    }
                    break;
                case PlayerState.Crouching:
                    if (frameInput.Move.y == 0 && !ceilingHit)
                    {
                        //transform.localScale = new Vector3(1, 1, 1);
                        capCollider.size = new Vector2(capCollider.size.x, 2);
                        MainPlayerRenderer.enabled = true;
                        HatRenderer.enabled = true;
                        CrouchingPlayer.gameObject.SetActive(false);
                        state = PlayerState.None;
                    }
                    break;
                case PlayerState.Handstand:
                    capCollider.size = new Vector2(capCollider.size.x, 2);
                    MainPlayerRenderer.enabled = true;
                    HatRenderer.enabled = true;
                    CrouchingPlayer.gameObject.SetActive(false);
                    //transform.localScale = new Vector3(1, 1, 1);
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
                case PlayerState.HorizontalBar:
                    if (frameInput.Move.y == -1)
                    {
                        state = PlayerState.None;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Up Input

        private float timeUpWasPressed;
        private float timeUpHasBeenHeld;

        public float getTimeUpHasBeenHeld()
        {
            return timeUpHasBeenHeld;
        }

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

                            SnapToRope(facing);
                        }
                    }
                    else if (inRangeOfBar)
                    {
                        if (frameInput.Move.y == 1)
                        {
                            velocity.x = 0;
                            velocity.y = 0;
                            state = PlayerState.HorizontalBar;
                            transform.position = new Vector3(transform.position.x,
                            climbable.GetRightTransform().y - moveVars.HorizontalRopeSnapPositionOffset,
                                transform.position.z);
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
                case PlayerState.HorizontalBar:
                    if (frameInput.UpDown)
                    {
                        timeUpWasPressed = time;
                    }

                    if (frameInput.UpHeld)
                    {
                        velocity.x = 0;

                        if (timeUpHasBeenHeld >= moveVars.MaxBarUpHoldTime)
                        {
                            timeUpHasBeenHeld = moveVars.MaxBarUpHoldTime;
                        }
                        else
                        {
                            timeUpHasBeenHeld = time - timeUpWasPressed;
                        }
                    }
                    else
                    {
                        if (timeUpHasBeenHeld <= 0)
                        {
                            timeUpHasBeenHeld = 0;
                        }
                        else
                        {
                            timeUpHasBeenHeld = timeUpHasBeenHeld - moveVars.BarSpeedDecay;
                        }
                    }
                    // Rotate player around bar at a speed scaled to how long up has been held

                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Pickups

        private PickUpBehvaior ObjectBeingHeld;

        public PickUpBehvaior GetObjectBeingHeld()
        {
            return ObjectBeingHeld;
        }

        private void HandlePickUp()
        {
            switch(state)
            {
                case PlayerState.None:
                    if (frameInput.PickUpDown && isOnTopOfPickup)
                    {
                        ObjectBeingHeld = ObjectOnTopOf.GetComponent<PickUpBehvaior>();
                        //ObjectBeingHeld.pickUpObject();
                        state = PlayerState.Carrying;
                        ObjectBeingHeld.beingCarried = true;
                    }
                    break;
                case PlayerState.Carrying:
                    if (frameInput.PickUpDown)
                    {
                        state = PlayerState.None;

                        if (frameInput.Move.y == 1)
                        {
                            ObjectBeingHeld.ThrowPickUp(new Vector2(0,
                                moveVars.ThrowingStrength + velocity.y));
                            Debug.Log("Object thrown");
                        }
                        else if (frameInput.Move.y == -1)
                        {
                            ObjectBeingHeld.ThrowPickUp(new Vector2(0, 0));
                            Debug.Log("Object thrown");
                        }
                        else if (frameInput.Move.x == -1)
                        {
                            ObjectBeingHeld.ThrowPickUp(new Vector2(velocity.x + -moveVars.ThrowingStrength,
                                moveVars.ThrowingStrength));
                            Debug.Log("Object thrown");
                        }
                        else if (frameInput.Move.x == 1)
                        {
                            ObjectBeingHeld.ThrowPickUp(new Vector2(velocity.x + moveVars.ThrowingStrength,
                                moveVars.ThrowingStrength));
                            Debug.Log("Object thrown");
                        }
                        else
                        {
                            switch (facing)
                            {
                                case PlayerDirection.Left:
                                    ObjectBeingHeld.ThrowPickUp(new Vector2(-moveVars.ThrowingStrength,
                                moveVars.ThrowingStrength / 2));
                                    Debug.Log("Object thrown");
                                    break;
                                case PlayerDirection.Right:
                                    ObjectBeingHeld.ThrowPickUp(new Vector2(moveVars.ThrowingStrength,
                                moveVars.ThrowingStrength / 2));
                                    Debug.Log("Object thrown");
                                    break;
                            }
                        }

                        ObjectBeingHeld = null;
                    }
                    break;
            }
        }

        #endregion

        #region Rope

        private void SnapToRope(PlayerDirection facing)
        {
            switch (facing)
            {
                case PlayerDirection.Left:
                    transform.position = new Vector3(climbable.GetLeftTransform().x + moveVars.VerticalRopeSnapPositionOffset, transform.position.y, transform.position.z);
                    break;
                case PlayerDirection.Right:
                    transform.position = new Vector3(climbable.GetRightTransform().x - moveVars.VerticalRopeSnapPositionOffset, transform.position.y, transform.position.z);
                    break;
            }
        }

        private void SnapToLeftRope()
        {
            transform.position = new Vector3(LeftClimbable.GetRightTransform().x + moveVars.VerticalRopeSnapPositionOffset, transform.position.y, transform.position.z);
        }

        private void SnapToRightRope()
        {
            transform.position = new Vector3(RightClimbable.GetLeftTransform().x - moveVars.VerticalRopeSnapPositionOffset, transform.position.y, transform.position.z);
        }

        private void SnapToMiddle()
        {
            float newX = (LeftClimbable.GetRightTransform().x - RightClimbable.GetLeftTransform().x) / 2 ;
            transform.position = new Vector3(LeftClimbable.GetRightTransform().x - newX, transform.position.y, transform.position.z);

        }

        #endregion

        #region Horizontal

        private bool HangingOffRope;
        private float timeDirectionChanged;

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
                        break;
                    case PlayerState.DoubleRope:
                        break;
                    case PlayerState.HorizontalBar:
                        velocity.x = Mathf.MoveTowards(velocity.x, frameInput.Move.x * moveVars.MaxHorizontalBarSpeed,
                            moveVars.Acceleration * Time.fixedDeltaTime);
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
                case PlayerState.SingleRope:
                    if (frameInput.RightDown)
                    {
                        facing = PlayerDirection.Left;
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (frameInput.LeftDown)
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
                case PlayerState.DoubleRope:
                    facing = PlayerDirection.Straight;
                    transform.rotation = Quaternion.Euler(0, 91, 0);
                    break;
                default:
                    if (frameInput.Move.x == -1)
                    {
                        if (facing == PlayerDirection.Right)
                        {
                            timeDirectionChanged = time;
                        }

                        facing = PlayerDirection.Left;
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (frameInput.Move.x == 1)
                    {
                        if (facing == PlayerDirection.Left)
                        {
                            timeDirectionChanged = time;
                        }

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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Single") || other.CompareTag("Horizontal"))
            {
                climbable = other.GetComponent<Climbable>();
            }

        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public bool PickUpDown; 
        public bool PickUpHeld;
        public bool UpDown;
        public bool UpHeld;
        public bool DownDown;
        public bool DownHeld;
        public bool LeftDown;
        public bool RightDown;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}
