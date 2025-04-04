using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Pickup
{
    private Transform playerTransform;
    private Rigidbody2D enemyRB;

    [SerializeField] private bool redShell = false;

    [SerializeField] private CircleCollider2D enemyDetectionSphere;
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private LayerMask groundLayer;

    public bool isDetectable = false;
    private bool isRight = false;

    private void Start()
    {
        initEnemy();

        enemyRB = GetComponent<Rigidbody2D>();
        //playerTransform = FindAnyObjectByType<PlayerMovement>().transform;
    }

    public void initEnemy()
    {
        initPickup(BaseType.NONE, transform.position);
    }

    private void Update()
    {
        //Collider2D col = Physics2D.OverlapCircle(enemyRB.position, enemyDetectionSphere.radius);
        //

        if (isDetectable)
        {
            detectMovement();
        }
        else
        {
            patrolMovement();
        }


    }

    //turns sprite around
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    //if chasing player
    private void detectMovement()
    {
        if (playerTransform.position.x < enemyRB.position.x)
        {
            enemyRB.linearVelocity = new Vector2(-1 * moveSpeed, enemyRB.linearVelocity.y);
            if (isRight)
            {
                Flip();
                isRight = false;
            }
        }
        else if (playerTransform.position.x > enemyRB.position.x)
        {
            enemyRB.linearVelocity = new Vector2(1 * moveSpeed, enemyRB.linearVelocity.y);
            if (!isRight)
            {
                Flip();
                isRight = true;
            }
        }
    }


    /* Creates a marker in front of the enemy that if doesnt detect ground turns around
     * is toggled off if player is detected
     */
    private void patrolMovement()
    {
        //if facing left
        if (!isRight)
        {
            if (redShell && !Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y - 1.1f), 0.2f, groundLayer))
            {
                updateVelocity(new Vector2(enemyRB.linearVelocity.x * -1, enemyRB.linearVelocity.y));
                Flip();
                isRight = true;
            }
            else
            {
                enemyRB.linearVelocity = new Vector2(-1 * moveSpeed, enemyRB.linearVelocity.y);
            }
        }
        //if facing right
        else if (isRight)
        {
            if (redShell && !Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x + 0.7f, gameObject.transform.position.y - 1.1f), 0.2f, groundLayer))
            {
                updateVelocity(new Vector2(enemyRB.linearVelocity.x * -1, enemyRB.linearVelocity.y));
                Flip();
                isRight = false;
            }
            else
            {
                enemyRB.linearVelocity = new Vector2(1 * moveSpeed, enemyRB.linearVelocity.y);
            }
        }

        
    }

    ////binds players movement to set platform
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" || collision.gameObject.CompareTag("PickUp"))
    //    {
    //        collision.gameObject.transform.SetParent(transform, true);
    //    }
    //}

    ////unbinds players movement
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" || collision.gameObject.CompareTag("PickUp"))
    //    {
    //        transform.DetachChildren();
    //    }
    //}

}
