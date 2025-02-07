using UnityEngine;

public class Fireball : Projectile
{
    
    //Constructors
    //Fireball class has no lifetime and is of fire material, it's permenant until it collides with something


    public void initFireball()
    {
        initProjectile(Material.Fire, false);
    }

    public void initFireball(Vector3 start, Vector2 v)
    {
        initProjectile(start, v, Material.Fire, false);
    }

    // Update is called once per frame
    void Update()
    {
        base.updateEntity(Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.GetComponent<Fireball>() != null)
        {
            //do nothing
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
