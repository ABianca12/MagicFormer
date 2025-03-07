using UnityEngine;

public class Crate : Pickup
{
    
    //member variables

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

    private void FixedUpdate()
    {
        base.fixedUpdateCall();
    }

    private void Update()
    {
        updatePickup();
    }
}
