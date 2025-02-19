using TarodevController;
using UnityEngine;

public class Climbable : Destructable
{
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

}
