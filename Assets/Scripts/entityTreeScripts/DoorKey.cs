using UnityEngine;

public class DoorKey : Pickup
{
    public void initKey()
    {
        base.initPickup(BaseType.MYSTIC, transform.position);
    }

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

    private void Start()
    {
        initKey();
    }

    private void Update()
    {
        //base.updatePickup();
    }

    private void FixedUpdate()
    {
        //base.fixedUpdateCall();
    }
}
