using System;
using TarodevController;
using UnityEngine;

public class PickUpBehvaior : MonoBehaviour
{
    public event Action<bool, float> GroundedChanged;

    public ThrowingVariables throwingVars;

    private GameObject player;
    private PlayerController controller;
    private Vector3 pickUpPos;
    public Vector3 initalPos;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    public bool beingCarried;
    private CapsuleCollider2D playerCapColl;
    [SerializeField]
    private Vector2 velocity;
    private bool startInColliders = false;
    public bool hasBeenThrown = false;
    private float time;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        throwingVars = GameObject.FindGameObjectWithTag("ThrowingVars").GetComponent<ThrowingVariables>();
        controller = player.GetComponent<PlayerController>();
        initalPos = transform.position;
        coll = GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        this.beingCarried = false;
        playerCapColl = player.GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        CheckCollisions();

        pickUpPos = new Vector3(player.transform.position.x,
            player.transform.position.y + playerCapColl.size.y,
            player.transform.position.z);

        if (controller.GetPlayerState() == PlayerController.PlayerState.Carrying && beingCarried)
        {
            this.transform.position = pickUpPos;
            hasBeenThrown = false;
        }
        else
        {
            this.beingCarried = false;
        }
    }

    private void FixedUpdate()
    {
        HandleGravity();
        HandleDirection();
        ApplyMovement();
    }

    private void HandleGravity()
    {
        if (grounded && velocity.y <= 0f)
        {
            velocity.y = throwingVars.GroundingForce;
        }
        else
        {
            var inAirGravity = throwingVars.FallAcceleration;
            velocity.y = Mathf.MoveTowards(velocity.y, -throwingVars.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    private bool grounded = true;

    private void CheckCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling
        bool groundHit = Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.down,
            throwingVars.PickUpGrounderDistance, throwingVars.defaultLayer);

        bool ceilingHit = Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.up,
            throwingVars.PickUpGrounderDistance, throwingVars.defaultLayer);

        bool leftHit = Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.left,
            throwingVars.PickUpGrounderDistance, throwingVars.defaultLayer);

        bool rightHit = Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.right,
            throwingVars.PickUpGrounderDistance, throwingVars.defaultLayer);

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
            var deceleration = grounded ? throwingVars.GroundDeceleration : throwingVars.AirDeceleration;
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
}
