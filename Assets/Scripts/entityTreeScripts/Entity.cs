using UnityEngine;

public class Entity : MonoBehaviour
{
    //Struct for materials
    public enum BaseType
    {
        NONE,
        METAL,
        WOOD,
        STONE,
        FIRE,
        ICE,
        GRASS,
        MYSTIC
    }

    //Member Variables
    protected Vector3 startPos;
    protected Vector3 currentPos;
    protected Vector2 velocity;
    protected BaseType material;
    [SerializeField] protected float zAxis;
    protected bool frozen = false;
    protected MaterialManager matManager;

    //Init functions
    public void initEntity(Vector3 start, Vector2 v, BaseType m)
    {
        startPos = start;
        currentPos = start;
        velocity = v;
        material = m;
        transform.position = startPos;
        matManager = GameObject.FindWithTag("MaterialManager").GetComponent<MaterialManager>();
    }
    public void initEntity(Vector3 start)
    {
        startPos = start;
        currentPos = start;
        velocity = Vector2.zero;
        material = BaseType.NONE;
        transform.position = startPos;
        matManager = GameObject.FindWithTag("MaterialManager").GetComponent<MaterialManager>();
    }
    public void initEntity(BaseType m)
    {
        startPos = Vector3.zero;
        currentPos = Vector3.zero;
        velocity = Vector2.zero;
        material = m;
        transform.position = startPos;
        matManager = GameObject.FindWithTag("MaterialManager").GetComponent<MaterialManager>();
    }
    public void initEntity(Vector3 sPos, BaseType m)
    {
        startPos = sPos;
        currentPos = sPos;
        velocity = Vector2.zero;
        material = m;
        transform.position = startPos;
        matManager = GameObject.FindWithTag("MaterialManager").GetComponent<MaterialManager>();
    }
    public void initEntity(Vector3 start, Vector2 v)
    {
        startPos = start;
        currentPos = start;
        velocity = v;
        material = BaseType.NONE;
        transform.position = startPos;
        matManager = GameObject.FindWithTag("MaterialManager").GetComponent<MaterialManager>();
    }
    public void initEntity()
    {
        startPos = Vector3.zero;
        currentPos = Vector3.zero;
        velocity = Vector2.zero;
        material = BaseType.NONE;
        transform.position = startPos;
        matManager = GameObject.FindWithTag("MaterialManager").GetComponent<MaterialManager>();
    }

    //Get function for material
    public BaseType getMaterial()
    {
        return material;
    }

    //updates position based on velocity
    public virtual void updateEntity(float deltaTime)
    {
        if(!frozen)
        {
            currentPos = new Vector3(currentPos.x + velocity.x * deltaTime, currentPos.y + velocity.y * deltaTime, zAxis);
            gameObject.transform.position = currentPos;
        }
    }

    //Function to edit velocity value
    public void updateVelocity(Vector2 newV)
    {
        velocity = newV;
    }

    //Function to update material of Entity
    public void setMaterial(BaseType newMaterial)
    {
        material = newMaterial;
    }

    //Function to add force to an object
    public virtual void addForce(Vector2 force, float strength = 2.0f)
    {
        Vector2 f = force.normalized;
        velocity += f * strength;
    }

    public void freeze()
    {
        frozen = true;
    }
    public void unfreeze()
    {
        frozen = false;
    }
}
