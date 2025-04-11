using UnityEngine;

public class Crate : Pickup
{

    //member variables
    private Rigidbody2D rb;

    //Init functions
    public void initCrate()
    {
        base.initPickup(BaseType.STONE);
    }
    public void initCrate(BaseType m)
    {
        base.initPickup(m);
    }
    public void initCrate(Vector3 start)
    {
        base.initPickup(BaseType.STONE, start);
    }
    public void initCrate(Vector3 start, BaseType b)
    {
        base.initPickup(b, start);
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
}
