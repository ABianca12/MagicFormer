using UnityEngine;

public class DoorKey : Pickup
{
    [SerializeField] private float lifetime = 10.0f;

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
        base.updateEntity(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        base.fixedUpdateCall();
    }
}
