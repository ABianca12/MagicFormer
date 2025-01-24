using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Member Variables
    private Vector3 startPos;
    private Vector3 currentPos;
    private Vector2 velocity;
    [SerializeField] private float zAxis;
    [SerializeField] private float lifeTime;

    //Constructor
    public Projectile(Vector3 start, Vector2 v)
    {
        startPos = start;
        currentPos = start;
        velocity = v;
        zAxis = start.z;
    }

    //Function updates projectiles
    public void updateProjectile()
    {
        this.transform.position = new Vector3(currentPos.x + velocity.x, currentPos.y + velocity.y, zAxis);
        
    }
}
