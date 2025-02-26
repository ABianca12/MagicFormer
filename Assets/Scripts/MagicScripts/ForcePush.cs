using UnityEngine;

public class ForcePush : Projectile
{
    //memeber variables
    [SerializeField] private float strength = 2.0f;

    //init functions
    public void initForcePush()
    {
        initProjectile(BaseType.MYSTIC, true, 0.5f);
    }
    public void initForcePush(Vector3 start)
    {
        initProjectile(BaseType.MYSTIC, true, 0.5f, start);
    }
    public void initForcePush(Vector3 start, Vector2 direction)
    {
        initProjectile(start, direction, BaseType.MYSTIC, true, 0.5f);
    }

    //Update is called once per frame
    void Update()
    {
        base.updateEntity(Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject g = collision.gameObject;
        //Checks if material can be pushed
        if (g.GetComponent<Entity>() == null)
        {
            //don't process collision
            Debug.Log("Collision Ignored");
        }
        else
        {
            //Check if material can be pushed
            if (g.GetComponent<Entity>().getMaterial() != BaseType.MYSTIC)
            {
                //Checks if object can be pushed
                if (g.GetComponent<Pickup>() != null || g.GetComponent<MovingPlatform>() != null)
                {
                    g.GetComponent<Entity>().addForce(base.velocity, strength);
                }
            }
        }
    }
}
