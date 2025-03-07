using UnityEngine;

public class MovementVariables : MonoBehaviour
{
    [Header("KEYBINDS")]
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode jump = KeyCode.Space;
    public KeyCode pickUp = KeyCode.R;

    [Header("LAYERS")]
    [Tooltip("Player Layer")]
    public LayerMask PlayerLayer;

    [Header("INPUT")]
    [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly. Recommended value is true to ensure gamepad/keyboard parity.")]
    public bool SnapInput = true;

    [Tooltip("Minimum input required before you mount a ladder or climb a ledge. Avoids unwanted climbing using controllers"), Range(0.01f, 0.99f)]
    public float VerticalDeadZoneThreshold = 0.3f;

    [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
    public float HorizontalDeadZoneThreshold = 0.1f;

    [Header("MOVEMENT")]
    [Tooltip("The top horizontal movement speed")]
    public float MaxSpeed = 14;

    [Tooltip("The top crouch movement speed")]
    public float MaxCrouchSpeed = 7;

    [Tooltip("The top handstand movement speed")]
    public float MaxHandStandSpeed = 7;

    [Tooltip("The top upwards movement speed when on a single rope")]
    public float MaxUpwardsSingleRopeSpeed = 4;

    [Tooltip("The top downwards movement speed when on a single rope")]
    public float MaxDownwardsSingleRopeSpeed = 14;

    [Tooltip("The top upwards movement speed when on a double rope")]
    public float MaxUpwardsDoubleRopeSpeed = 14;

    [Tooltip("The top downwards movement speed when on a double rope")]
    public float MaxDownwardsDoubleRopeSpeed = 4;

    [Tooltip("The top horizontal movement speed when on a horizontal bar")]
    public float MaxHorizontalBarSpeed = 5;

    [Tooltip("The player's capacity to gain horizontal speed")]
    public float Acceleration = 120;

    [Tooltip("The player's capacity to gain horizontal speed while crouching")]
    public float CrouchAcceleration = 120;

    [Tooltip("The player's capacity to gain horizontal speed while in a handstand")]
    public float HandstandAcceleration = 120;

    [Tooltip("The pace at which the player comes to a stop")]
    public float GroundDeceleration = 60;

    [Tooltip("Deceleration in air only after stopping input mid-air")]
    public float AirDeceleration = 30;

    [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
    public float GroundingForce = -1.5f;

    [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
    public float GrounderDistance = 0.05f;

    [Header("JUMP")]
    [Tooltip("The immediate velocity applied when jumping")]
    public float JumpPower = 36;

    [Tooltip("The immediate velocity applied when transitioning to a handstand")]
    public float HandStandTransitionJumpPower = 18;

    [Tooltip("The immediate velocity applied when handstand jumping")]
    public float HandStandJumpPower = 50;

    [Tooltip("The immediate velocity applied when fully charged horizontal bar jumping")]
    public float BarJumpPower = 10;

    [Tooltip("The maximum vertical movement speed")]
    public float MaxFallSpeed = 40;

    [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
    public float FallAcceleration = 110;

    [Tooltip("The gravity multiplier added when jump is released early")]
    public float JumpEndEarlyGravityModifier = 3;

    [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
    public float CoyoteTime = .15f;

    [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
    public float JumpBuffer = .2f;

    [Header("Throwing")]
    [Tooltip("The full force of throwing something")]
    public float ThrowingStrength = 100.0f;

    [Header("Climbing")]
    [Tooltip("The offset of the player's position when snaping to a rope"), Range(0.01f, 0.99f)]
    public float RopeSnapPositionOffset = 0.5f;
}
