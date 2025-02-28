using UnityEngine;

public class Pickup : Destructable
{
    //Member variables
    private bool grounded = true;

    [Tooltip("The pace at which the throwable comes to a stop")]
    public float GroundDeceleration = 60;

    [Tooltip("Deceleration in air only after stopping input mid-air")]
    public float AirDeceleration = 30;

    //Init functions
    public void initPickup()
    {
        base.initDestructable();
    }
    public void initPickup(BaseType m)
    {
        base.initDestructable(m);
    }
    public void initPickup(BaseType m, Vector3 start)
    {
        base.initDestructable(m, start);
    }

    public void throwPickup(Vector2 direction)
    {
        base.updateVelocity(direction);
    }

    private void HandleDirection()
    {
        if (velocity.x > 0 || velocity.x < 0)
        {
            var deceleration = grounded ? GroundDeceleration : AirDeceleration;
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {

        }
    }
}
