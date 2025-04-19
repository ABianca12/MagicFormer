using System;
using TarodevController;
using UnityEngine;
using static TarodevController.PlayerController;

public class Pickup : Destructable
{
    public event Action<bool, float> GroundedChanged;

    public ThrowingVariables throwingVars;

    private GameObject player;
    private PlayerController controller;
    private Vector3 pickUpPos;
    public Vector3 initalPos;
    private Renderer rend;
    //private BoxCollider2D[] allColliders;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    public bool beingCarried;
    private CapsuleCollider2D playerCapColl;
    [SerializeField]
    private Vector2 velocity;
    private bool startInColliders = false;
    public bool hasBeenThrown = false;
    private float time;

    //Init functions
    protected void initMemberVars()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        throwingVars = GameObject.FindGameObjectWithTag("ThrowingVars").GetComponent<ThrowingVariables>();
        controller = player.GetComponent<PlayerController>();
        initalPos = transform.position;
        rend = this.GetComponent<Renderer>();
        //allColliders = this.GetComponents<BoxCollider2D>();
        coll = GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        this.beingCarried = false;
        playerCapColl = player.GetComponent<CapsuleCollider2D>();
    }

    public void initPickup()
    {
        base.initDestructable();
        initMemberVars();
    }
    public void initPickup(BaseType m)
    {
        base.initDestructable(m);
        initMemberVars();
    }
    public void initPickup(BaseType m, Vector3 start)
    {
        base.initDestructable(m, start);
        initMemberVars();
    }

}
