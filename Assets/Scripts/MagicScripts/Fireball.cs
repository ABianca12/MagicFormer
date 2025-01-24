using UnityEngine;

public class Fireball : Projectile
{
    
    //Constructors
    //Fireball class has no lifetime and is of fire material, it's permenant until it collides with something
    public Fireball() : base(Material.Fire, false)
    {

    }

    //public Fireball(Vector3 start, )

    // Update is called once per frame
    void Update()
    {
        
    }
}
