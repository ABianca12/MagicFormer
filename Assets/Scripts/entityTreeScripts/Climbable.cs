using TarodevController;
using UnityEngine;

public class Climbable : Destructable
{
    private CapsuleCollider2D capColl;
    private Vector3 RightTransform;
    private Vector3 LeftTransform;

    private void Start()
    {
        capColl = GetComponent<CapsuleCollider2D>();
        this.LeftTransform = capColl.bounds.max;
        this.RightTransform = capColl.bounds.min;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.CompareTag("Single"))
            {
                PlayerController.setInRangeOfRope(true);
            }
            else if (transform.CompareTag("Horizontal"))
            {
                PlayerController.setInRangeOfBar(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.CompareTag("Single"))
            {
                PlayerController.setInRangeOfRope(false);
            }
            else if (transform.CompareTag("Horizontal"))
            {
                PlayerController.setInRangeOfBar(false);
            }
        }
    }

    public Vector3 GetLeftTransform()
    {
        return LeftTransform;
    }

    public Vector3 GetRightTransform()
    {
        Debug.Log(RightTransform);
        return RightTransform;
    }
}
