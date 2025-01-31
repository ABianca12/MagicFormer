using UnityEngine;

public class Projectile : Entity
{
    //Member Variables
    [SerializeField] protected float lifeTime = 10;
    [SerializeField] protected bool temporary;

    //Constructors
    public void initProjectile(Vector3 start, Vector2 v, Material m, float l)
    {
        initEntity(start, v, m);
        lifeTime = l;
    }
    public void initProjectile(Vector3 start, Vector2 v, Material m, bool temp)
    {
        initEntity(start, v, m);
        temporary = temp;
    }
    public void initProjectile(Vector3 start, Vector2 v, Material m)
    {
        initEntity(start, v, m);
    }
    public void initProjectile(Material m, bool temp)
    {
        initEntity(m);
        temporary = temp;
    }
    public void initProjectile(Material m)
    {
        initEntity(m);
    }
    public void initProjectile()
    {
        initEntity();
    }

    //Function updates projectiles
    public void updateEntity(float deltaTime)
    {
        base.updateEntity();
        //this.transform.position = new Vector3(currentPos.x + velocity.x, currentPos.y + velocity.y, zAxis);
        //Reduces lifetime if the projectile has a lifespan
        if(temporary)
        {
            lifeTime -= deltaTime;
            //Destroys projectile if lifetime is expired
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
