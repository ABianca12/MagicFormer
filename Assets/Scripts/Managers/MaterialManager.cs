using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void handleCollision(Entity collider, Entity collidee)
    {
        //Check for directly destructable interactions
        Destructable c2 = collidee.GetComponent<Destructable>();
        if (c2 != null)
        {
            switch(collider.getMaterial())
            {
                case Entity.BaseType.FIRE:
                    //Fire object kills enemies
                    if(collidee.gameObject.GetComponent<Enemy>() != null)
                    {
                        c2.destroyObject();
                    }
                    //Fire object destroys wood, grass, and ice
                    if(collidee.getMaterial() == Entity.BaseType.WOOD || collidee.getMaterial() == Entity.BaseType.GRASS || collidee.getMaterial() == Entity.BaseType.ICE)
                    {
                        c2.destroyObject();
                    }
                break;
                default:
                    //do nothing
                break;
            }
        }
    }
}
