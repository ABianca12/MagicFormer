using UnityEngine;
using System.Collections.Generic;

public class Timestop : Projectile
{
    //member variables
    private List<Entity> objectsFrozen = new List<Entity>();
    private float radius = 2.0f;

    //init functions
    public void initTimestop()
    {
        initProjectile(BaseType.MYSTIC, false);
    }
    public void initTimestop(Vector3 startPos)
    {
        initProjectile(startPos, BaseType.MYSTIC, false);
    }

    //Update is called every frame
    void Update()
    {

    }

    public void releaseTimestop()
    {
        //Release all objectsFrozen from stasis

        Destroy(gameObject);
    }
}
