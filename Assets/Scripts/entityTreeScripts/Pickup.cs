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
    private bool thrown;
    private bool grounded;
    private CapsuleCollider2D playerCapColl;
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
        playerCapColl = player.GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        pickUpPos = new Vector3(player.transform.position.x,
            player.transform.position.y + playerCapColl.size.y,
            player.transform.position.z);

        if (controller.GetPlayerState() == PlayerController.PlayerState.Carrying)
        {
            transform.position = pickUpPos;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
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
