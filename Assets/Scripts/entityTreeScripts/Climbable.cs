using TarodevController;
using UnityEngine;

public class Climbable : Destructable
{
    public PlayerController player;

    private CapsuleCollider2D capColl;
    private Vector3 RightTransform;
    private Vector3 LeftTransform;

    private void Start()
    {
        capColl = GetComponent<CapsuleCollider2D>();
        player = FindAnyObjectByType<PlayerController>();
        this.LeftTransform = capColl.bounds.max;
        this.RightTransform = capColl.bounds.min;
    }

    public void initClimbable()
    {
        base.initDestructable();
    }
    public void initClimbable(BaseType m)
    {
        base.initDestructable(m);
    }
    public void initClimbable(BaseType m, Vector3 start)
    {
        base.initDestructable(m, start);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.CompareTag("Single"))
            {
                player.setInRangeOfRope(true);
            }
            else if (transform.CompareTag("Horizontal"))
            {
                player.setInRangeOfBar(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.CompareTag("Single"))
            {
                player.setInRangeOfRope(false);
            }
            else if (transform.CompareTag("Horizontal"))
            {
                player.setInRangeOfBar(false);
            }
        }
    }

    public Vector3 GetLeftTransform()
    {
        return LeftTransform;
    }

    public Vector3 GetRightTransform()
    {
        return RightTransform;
    }
}
