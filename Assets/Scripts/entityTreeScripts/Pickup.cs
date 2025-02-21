using System.Runtime.CompilerServices;
using TarodevController;
using UnityEngine;
using static TarodevController.PlayerController;

public class Pickup : Destructable
{
    public float PickUpGrounderDistance = 0.05f;
    public float GroundingForce = -1.5f;
    public float MaxFallSpeed = 40;
    public float FallAcceleration = 110;

    private GameObject player;
    private PlayerController controller;
    private Vector3 pickUpPos;
    private Vector3 initalPos;
    private Renderer rend;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private bool beingCarried;
    private bool beingThrown;
    private bool grounded;
    //private Vector2 velocity;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        initalPos = transform.position;
        rend = this.GetComponent<Renderer>();
        coll = this.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Update()
    {
        pickUpPos = new Vector3(player.transform.position.x, player.transform.position.y + 2,
            player.transform.position.z);

        if (controller.GetPlayerState() == PlayerController.PlayerState.Carrying)
        {
            transform.position = pickUpPos;
            Debug.Log("Updated pos");
            //Instantiate(this.gameObject, player.transform.position, player.transform.rotation);
        }
        

    }

    private void FixedUpdate()
    {
        Collisions();

        if (beingThrown)
        {
            HandleGravity();
            ApplyMovement();
        }
    }

    void Collisions()
    {
        bool groundHit = Physics2D.BoxCast(coll.bounds.center, coll.size,
                0, Vector2.down, PickUpGrounderDistance);

        bool ceilingHit = Physics2D.BoxCast(coll.bounds.center, coll.size,
                0, Vector2.up, PickUpGrounderDistance);

        // Hit a Ceiling
        if (ceilingHit && !beingCarried)
        {
            velocity.y = Mathf.Min(0, velocity.y);
        }

        // Landed on the Ground
        if (!grounded && groundHit)
        {
            grounded = true;
            //GroundedChanged?.Invoke(true, Mathf.Abs(velocity.y));
        }
        // Left the Ground
        else if (grounded && !groundHit)
        {
            grounded = false;
            //GroundedChanged?.Invoke(false, 0);
        }
    }

    public void throwPickUp(Vector2 direction, float force)
    {
        rb.linearVelocity = direction;
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

    private void ApplyMovement() => rb.linearVelocity = velocity;
}
