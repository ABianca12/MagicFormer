using TarodevController;
using UnityEngine;

public class BounceBlock : Ground
{

    public int bounceForce = 100;
    [Range(1,2)]
    public float bounceMultiplier = 1.5f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(Input.GetKey(KeyCode.Space))
            {
                collision.gameObject.GetComponent<PlayerController>().velocity += new Vector2(0, bounceForce * bounceMultiplier);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().velocity += new Vector2(0, bounceForce);
            }
            
        }
    }
}
