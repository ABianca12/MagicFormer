using UnityEngine;

public class Crate : Pickup
{

    //member variables
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D collider;

    //Init functions
    public void initCrate()
    {
        checkSpawn();
        base.initPickup(BaseType.STONE);
    }
    public void initCrate(BaseType m)
    {
        checkSpawn();
        base.initPickup(m);
    }
    public void initCrate(Vector3 start)
    {
        checkSpawn();
        base.initPickup(BaseType.STONE, start);
    }
    public void initCrate(Vector3 start, BaseType b)
    {
        checkSpawn();
        base.initPickup(b, start);
        
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        collider.isTrigger = false;
    }

    private void FixedUpdate()
    {

    }

    private void Update()
    {
        if(frozen)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void checkSpawn()
    {
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Pickup")
        {
            Destroy(gameObject);
        }
    }
}
