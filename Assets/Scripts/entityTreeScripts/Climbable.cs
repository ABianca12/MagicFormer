using TarodevController;
using UnityEngine;

public class Climbable : Destructable
{
    private CapsuleCollider2D capColl;
    private float RightTransform;
    private float LeftTransform;

    private void Start()
    {
        capColl = GetComponent<CapsuleCollider2D>();
        this.LeftTransform = capColl.bounds.max.x;
        this.RightTransform = capColl.bounds.min.x;
        Debug.Log(capColl.bounds.max);
        Debug.Log(capColl.bounds.min);
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

    public float GetLeftTransform()
    {
        return LeftTransform;
    }

    public float GetRightTransform()
    {
        return RightTransform;
    }
}
