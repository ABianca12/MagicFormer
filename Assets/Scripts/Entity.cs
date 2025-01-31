using UnityEngine;

public class Entity : MonoBehaviour
{
    //Struct for materials
    public enum Material
    {
        None,
        Metal,
        Wood,
        Stone,
        Fire,
        Ice,
        Grass
    }

    //Member Variables
    protected Vector3 startPos;
    protected Vector3 currentPos;
    protected Vector2 velocity;
    protected Material material;
    [SerializeField] protected float zAxis;

    public void initEntity(Vector3 start, Vector2 v, Material m)
    {
        startPos = start;
        currentPos = start;
        velocity = v;
        material = m;
    }
    public void initEntity(Vector3 start)
    {
        startPos = start;
        currentPos = start;
        velocity = Vector2.zero;
        material = Material.None;
    }
    public void initEntity(Material m)
    {
        startPos = Vector3.zero;
        currentPos = Vector3.zero;
        velocity = Vector2.zero;
        material = m;
    }
    public void initEntity(Vector3 start, Vector2 v)
    {
        startPos = start;
        currentPos = start;
        velocity = v;
        material = Material.None;
    }
    public void initEntity()
    {
        startPos = Vector3.zero;
        currentPos = Vector3.zero;
        velocity = Vector2.zero;
        material = Material.None;
    }

    //updates position based on velocity
    public void updateEntity(float deltaTime)
    {
        currentPos = new Vector3(currentPos.x + velocity.x*deltaTime, currentPos.y + velocity.y*deltaTime, zAxis);
        gameObject.transform.position = currentPos;
    }

    //Function to edit velocity value
    public void updateVelocity(Vector2 newV)
    {
        velocity = newV;
    }
}
