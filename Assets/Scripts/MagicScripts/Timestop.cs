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
    public void initTimestop(Vector3 startPos, float rad)
    {
        radius = rad;
        initProjectile(startPos, BaseType.MYSTIC, false);
    }

    //Start is called before the first frame
    void Start()
    {
        //gameObject.transform.Scale = new Vector3(radius, radius, 1);
        List<Collider2D> list = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.NoFilter();
        Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), radius, contactFilter, list);
        foreach (Collider2D collider in list)
        {
            Entity e = collider.gameObject.GetComponent<Entity>();
            if (e != null)
            {
                e.freeze();
                objectsFrozen.Add(e);
            }
        }
    }

    //Update is called every frame
    void Update()
    {

    }

    public void releaseTimestop()
    {
        //Release all objectsFrozen from stasis
        foreach(Entity ent in objectsFrozen)
        {
            ent.unfreeze();
        }
        Destroy(gameObject);
    }
}
