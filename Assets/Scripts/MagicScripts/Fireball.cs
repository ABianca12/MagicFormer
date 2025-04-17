using UnityEngine;

public class Fireball : Projectile
{

    //Constructors
    //Fireball class has no lifetime and is of fire material, it's permenant until it collides with something
    [SerializeField] private ParticleSystem finale;

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
        if(collision.gameObject.tag == "Player" || collision.gameObject.GetComponent<Fireball>() != null || collision.gameObject.GetComponent<Timestop>() != null)
        {
            //do nothing
            Debug.Log("Collision Ignored");
        }
        else
        {
            //add fire particle fx
            Instantiate(finale, gameObject.transform.position, Quaternion.identity);

            //checks if colliding with other entity, if so calls the handle collision script
            if (collision.gameObject.GetComponent<Entity>() != null)
            {
                matManager.handleCollision(this, collision.gameObject.GetComponent<Entity>());
            }
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.GetComponent<Fireball>() != null || collision.gameObject.GetComponent<Timestop>() != null)
        {
            //do nothing
            Debug.Log("Collision Ignored");
        }
        else
        {
            //add fire particle fx
            Instantiate(finale, gameObject.transform.position, Quaternion.identity);

            //checks if colliding with other entity, if so calls the handle collision script
            if (collision.gameObject.GetComponent<Entity>() != null)
            {
                matManager.handleCollision(this, collision.gameObject.GetComponent<Entity>());
            }
            Destroy(gameObject);
        }

    }
}
