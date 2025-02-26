using UnityEngine;

public class Fireball : Projectile
{
    
    //Constructors
    //Fireball class has no lifetime and is of fire material, it's permenant until it collides with something


    public void initFireball()
    {
        initProjectile(BaseType.FIRE, false);
    }

    public void initFireball(Vector3 start, Vector2 v)
    {
        initProjectile(start, v, BaseType.FIRE, false);
    }

    // Update is called once per frame
    void Update()
    {
        base.updateEntity(Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.GetComponent<Fireball>() != null)
        {
            //do nothing
            Debug.Log("Collision Ignored");
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
