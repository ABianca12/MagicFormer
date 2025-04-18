using UnityEngine;

public class Projectile : Entity
{
    //Member Variables
    [SerializeField] protected float lifeTime = 10;
    [SerializeField] protected bool temporary;

    //Constructors
    public void initProjectile(Vector3 start, Vector2 v, BaseType m, float l)
    {
        initEntity(start, v, m);
        lifeTime = l;
    }
    public void initProjectile(Vector3 start, Vector2 v, BaseType m, bool temp)
    {
        initEntity(start, v, m);
        temporary = temp;
    }
    public void initProjectile(Vector3 start, Vector2 v, BaseType m, bool temp, float l)
    {
        initEntity(start, v, m);
        temporary = temp;
        lifeTime = l;
    }
    public void initProjectile(Vector3 start, Vector2 v, BaseType m)
    {
        initEntity(start, v, m);
    }
    public void initProjectile(BaseType m, bool temp)
    {
        initEntity(m);
        temporary = temp;
    }
    public void initProjectile(Vector3 start, BaseType m, bool temp)
    {
        initEntity(start, m);
        temporary = temp;
    }
    public void initProjectile(BaseType m, bool temp, float life)
    {
        initEntity(m);
        temporary = temp;
        lifeTime = life;
    }
    public void initProjectile(BaseType m, bool temp, float life, Vector3 start)
    {
        initEntity(start, m);
        temporary = temp;
        lifeTime = life;
    }
    public void initProjectile(BaseType m)
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
        base.updateEntity(deltaTime);
        //this.transform.position = new Vector3(currentPos.x + velocity.x, currentPos.y + velocity.y, zAxis);
        //Reduces lifetime if the projectile has a lifespan
        if(temporary)
        {
            if(!frozen)
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
}
