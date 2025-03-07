using TarodevController;
using UnityEngine;

public class Climbable : Destructable
{
    private CapsuleCollider2D capColl;
    private static float RightTransform;
    private static float LeftTransform;

    private void Start()
    {
        capColl = GetComponent<CapsuleCollider2D>();
        LeftTransform = capColl.bounds.max.x;
        RightTransform = capColl.bounds.min.x;
        Debug.Log(capColl.bounds.max);
        Debug.Log(capColl.bounds.min);
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
            PlayerController.setInRangeOfRope(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.setInRangeOfRope(false);
        }
    }

    public static float GetLeftTransform()
    {
        return LeftTransform;
    }

    public static float GetRightTransform()
    {
        return RightTransform;
    }
}
