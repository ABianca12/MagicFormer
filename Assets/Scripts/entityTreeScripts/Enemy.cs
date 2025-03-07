using UnityEngine;

public class Enemy : Pickup
{
    private Transform playerTransform;

    [SerializeField] private Rigidbody2D enemyRB;
    [SerializeField] private CircleCollider2D enemyDetectionSphere;
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private LayerMask groundLayer;

    public bool isDetectable = false;
    private bool isRight = false;

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        playerTransform = FindAnyObjectByType<PlayerMovement>().transform;
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
            if (!Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y - 1.1f), 0.2f, groundLayer))
            {
                enemyRB.linearVelocity = new Vector2(enemyRB.linearVelocity.x * -1, enemyRB.linearVelocity.y);
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
            if (!Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x + 0.7f, gameObject.transform.position.y - 1.1f), 0.2f, groundLayer))
            {
                enemyRB.linearVelocity = new Vector2(enemyRB.linearVelocity.x * -1, enemyRB.linearVelocity.y);
                Flip();
                isRight = false;
            }
            else
            {
                enemyRB.linearVelocity = new Vector2(1 * moveSpeed, enemyRB.linearVelocity.y);
            }
        }
    }
}
