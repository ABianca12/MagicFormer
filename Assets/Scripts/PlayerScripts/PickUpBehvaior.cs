using System;
using TarodevController;
using UnityEngine;

public class PickUpBehvaior : MonoBehaviour
{
    public float PickUpGrounderDistance = 0.05f;
    public float GroundingForce = -1.5f;
    public float MaxFallSpeed = 80;
    public float FallAcceleration = 150;
    public float keyResetTime = 10.0f;
    private float currentTimerTime;
    public event Action<bool, float> GroundedChanged;
    public LayerMask defaultLayer;
    public LayerMask playerLayer;
    public LayerMask pickUpLayer;

    [Tooltip("The pace at which the throwable comes to a stop")]
    public float GroundDeceleration = 60;

    [Tooltip("Deceleration in air only after stopping input mid-air")]
    public float AirDeceleration = 30;

    private GameObject player;
    private PlayerController controller;
    private Vector3 pickUpPos;
    private Vector3 initalPos;
    private Renderer rend;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    public bool beingCarried;
    private CapsuleCollider2D playerCapColl;
    [SerializeField]
    private Vector2 velocity;
    private bool startInColliders = false;
    private bool hasBeenThrown = false;
    private float time;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        initalPos = transform.position;
        rend = this.GetComponent<Renderer>();
        coll = this.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        this.beingCarried = false;
        playerCapColl = player.GetComponent<CapsuleCollider2D>();

        currentTimerTime = keyResetTime;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (hasBeenThrown /*&& velocity.x == 0 && velocity.y == -MaxFallSpeed*/)
        {
            UpdateTimer();
        }

        pickUpPos = new Vector3(player.transform.position.x,
            player.transform.position.y + playerCapColl.size.y,
            player.transform.position.z);

        if (controller.GetPlayerState() == PlayerController.PlayerState.Carrying && beingCarried)
        {
            this.transform.position = pickUpPos;
            hasBeenThrown = false;
            currentTimerTime = keyResetTime;
        }
        else
        {
            this.beingCarried = false;
        }
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        HandleGravity();
        HandleDirection();
        ApplyMovement();
    }

    private void HandleGravity()
    {
        if (grounded && velocity.y <= 0f)
        {
            velocity.y = GroundingForce;
        }
        else
        {
            var inAirGravity = FallAcceleration;
            velocity.y = Mathf.MoveTowards(velocity.y, -MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    private bool grounded = true;

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling
        bool groundHit = Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.down,
            PickUpGrounderDistance, defaultLayer);

        bool ceilingHit = Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.up,
            PickUpGrounderDistance, defaultLayer);

        bool leftHit = Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.left,
            PickUpGrounderDistance, defaultLayer);

        bool rightHit = Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.right,
            PickUpGrounderDistance, defaultLayer);

        // Hit a Ceiling
        if (ceilingHit)
        {
            velocity.y = Mathf.Min(0, velocity.y);
        }

        if (leftHit || rightHit)
        {
            velocity.x = 0;
        }

        // Landed on the Ground
        if (!grounded && groundHit)
        {
            grounded = true;
            velocity.x = 0;
            GroundedChanged?.Invoke(true, Mathf.Abs(velocity.y));
        }
        // Left the Ground
        else if (grounded && !groundHit)
        {
            grounded = false;
            GroundedChanged?.Invoke(false, 0);
        }

        Physics2D.queriesStartInColliders = startInColliders;
    }

    private void HandleDirection()
    {
        if (velocity.x > 0 || velocity.x < 0)
        {
            var deceleration = grounded ? GroundDeceleration : AirDeceleration;
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
    }

    public void ThrowPickUp(Vector2 direction)
    {
        velocity.x = direction.x;
        velocity.y = direction.y;
        hasBeenThrown = true;
    }

    private void ApplyMovement() => rb.linearVelocity = velocity;

    private void UpdateTimer()
    {
        currentTimerTime -= Time.deltaTime;

        if (currentTimerTime <= 0.0)
        {
            TimerEnded();
        }
    }

    private void TimerEnded()
    {
        transform.position = initalPos;
        currentTimerTime = keyResetTime;
        hasBeenThrown = false;
    }
}
