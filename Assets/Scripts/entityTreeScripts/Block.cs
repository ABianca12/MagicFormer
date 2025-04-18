using System;
using UnityEngine;

public class Block : Interactable
{
    [SerializeField] protected BaseType tahp = BaseType.STONE;
    [SerializeField] protected float friction = 0.0f;
    [SerializeField] protected float gravity = 1f;
    protected Collider2D col;

    public void initBlock()
    {
        initInteractable();
    }
    public void initBlock(Vector3 start)
    {
        initInteractable(start);
    }
    public void initBlock(Vector3 start, BaseType m)
    {
        initInteractable(start, m);
    }

    private void Start()
    {
        initBlock(gameObject.transform.position, tahp);
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        updateEntity(Time.deltaTime);
        Vector2 full = new Vector2(Mathf.Max(MathF.Abs(base.velocity.x) - friction, 0), Mathf.Max(Mathf.Abs(base.velocity.y), 0));
        //Raycast for down
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - transform.localScale.y / 2), -Vector2.up, 0.1f);
        if (hit)
        {
            if(hit.collider.gameObject != gameObject)
            {
                base.velocity = new Vector2(full.x * Mathf.Sign(base.velocity.x), 0);
            }
        }
        else
        {
            base.velocity = new Vector2(full.x * Mathf.Sign(base.velocity.x), (full.y * Mathf.Sign(base.velocity.y)) - gravity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.GetComponent<Projectile>() == null)
        {
            if(Physics2D.Raycast(collision.GetContact(0).point - new Vector2(1,0), new Vector2(1,0), 1.1f))
            {
                base.velocity.x = 0;
                base.currentPos.x -= 0.2f;
            }
            else if(Physics2D.Raycast(collision.GetContact(0).point + new Vector2(1, 0), new Vector2(-1, 0), 1.1f))
            {
                base.velocity.x = 0;
                base.currentPos.x += 0.2f;
                Debug.Log("STOP X");
            }

            if (Physics2D.Raycast(collision.GetContact(0).point - new Vector2(0,1), new Vector2(0, 1), 0.1f))
            {
                base.velocity.y = 0;
                base.currentPos.y -= 0.2f;
            }
            else if(Physics2D.Raycast(collision.GetContact(0).point + new Vector2(0, 1), new Vector2(0, -1), 0.1f))
            {
                base.velocity.y = 0;
                base.currentPos.y += 0.2f;
            }
        }
    }
}
