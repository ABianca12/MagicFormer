using UnityEngine;

public class Projectile : Entity
{
    //Member Variables
    [SerializeField] protected float lifeTime = 10;
    [SerializeField] protected bool temporary;

    //Constructors
    public Projectile(Vector3 start, Vector2 v, Material m, float l) : base(start, v, m)
    {
        lifeTime = l;
    }
    public Projectile(Vector3 start, Vector2 v, Material m) : base(start, v, m)
    {

    }
    public Projectile(Material m, bool temp) : base(m)
    {
        temporary = temp;
    }
    public Projectile(Material m) : base(m)
    {

    }
    public Projectile() : base()
    {

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
