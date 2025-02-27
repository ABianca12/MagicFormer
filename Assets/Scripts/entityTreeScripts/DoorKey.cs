using UnityEngine;

public class DoorKey : Pickup
{
    [SerializeField] private float lifetime = 10.0f;



    public override void destroyObject()
    {
        gameObject.transform.position = base.startPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Spikes>() != null)
        {
            destroyObject();
        }
    }
}
