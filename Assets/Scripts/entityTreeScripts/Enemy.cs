using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Pickup
{
    private Transform playerTransform;
    private Rigidbody2D enemyRB;

    [SerializeField] private bool redShell = false;

    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private GameObject enemyHitbox;
    [SerializeField] private GameObject attackCanvas;

    public bool isAttacking = false;
    private bool isRight = false;
    private float attackCooldownTimer = 0;

    private void Start()
    {
        initEnemy();

        enemyRB = GetComponent<Rigidbody2D>();

        enemyRB.linearVelocity = new Vector2(-1 * moveSpeed, enemyRB.linearVelocity.y);
    }

    public void initEnemy()
    {
        initPickup(BaseType.NONE, transform.position);
    }

    private void Update()
    {
        //Collider2D col = Physics2D.OverlapCircle(enemyRB.position, enemyDetectionSphere.radius);
        //
        PatrolMovement();
    }

    //turns sprite around
    public void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;

        updateVelocity(new Vector2(-1 * enemyRB.linearVelocity.x, enemyRB.linearVelocity.y));
        enemyRB.linearVelocity = new Vector2(-1 * enemyRB.linearVelocity.x, enemyRB.linearVelocity.y);
        isRight = !isRight;
    }

    /* Creates a marker in front of the enemy that if doesnt detect ground turns around
     * is toggled off if player is detected
     */
    private void PatrolMovement()
    {
        WallDetection();
        GroundDetection();

        Attack();

        if(enemyRB.linearVelocity.x == 0 && !isAttacking)
        {
            if(isRight)
            {
                enemyRB.linearVelocityX = moveSpeed;
            }
            else
            {
                enemyRB.linearVelocityX = -moveSpeed;
            }
        }
    }

    //checks ground in front of enemy to know if should turn around
    private void GroundDetection()
    {
        if (!isRight)
        {
            if (redShell && !Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y - 1.1f), 0.2f, groundLayer))
            {
                Flip();
            }
        }
        else if (isRight)
        {
            if (redShell && !Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x + 0.7f, gameObject.transform.position.y - 1.1f), 0.2f, groundLayer))
            {
                Flip();
            }
        }
    }

    //checks if anything is infront of enemy and turns it around if so
    private void WallDetection()
    {
        if(isRight)
        {
            if(Physics2D.OverlapArea(new Vector2(gameObject.transform.position.x + 0.7f, gameObject.transform.position.y - 0.9f),
                new Vector2(gameObject.transform.position.x + 0.7f, gameObject.transform.position.y + 0.9f), 0))
            {
                Flip();
            }
        }
        else
        {
            if (Physics2D.OverlapArea(new Vector2(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y - 0.9f),
                new Vector2(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y + 0.9f), 0))
            {
                Flip();
            }
        }
    }
    private void Attack()
    {
        if (isRight)
        {
            if (Physics2D.OverlapArea(new Vector2(gameObject.transform.position.x + 2f, gameObject.transform.position.y - 0.9f),
                new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y + 0.9f), playerLayer))
            {
                attackCooldownTimer += Time.deltaTime;
                updateVelocity(new Vector2(0, enemyRB.linearVelocity.y));
                enemyRB.linearVelocity = new Vector2(0, enemyRB.linearVelocity.y);
                isAttacking = true;
                attackCanvas.SetActive(true);
                //after .75f set hitbox active
                switch (attackCooldownTimer)
                {
                    case < 0.75f:
                        break;
                    case < 2f:
                        enemyHitbox.SetActive(true);
                        break;
                    default:
                        enemyHitbox.SetActive(false);
                        attackCooldownTimer = 0;
                        break;
                }
            }
            else
            {
                attackCanvas.SetActive(false);
                isAttacking = false;
                enemyHitbox.SetActive(false);
                attackCooldownTimer = 0;
            }
        }
        else if(!isRight)
        {
            if (Physics2D.OverlapArea(new Vector2(gameObject.transform.position.x - 1.3f, gameObject.transform.position.y - 0.9f),
                new Vector2(gameObject.transform.position.x - 0.2f, gameObject.transform.position.y + 0.9f), playerLayer))
            {
                attackCooldownTimer += Time.deltaTime;
                updateVelocity(new Vector2(0, enemyRB.linearVelocity.y));
                enemyRB.linearVelocity = new Vector2(0, enemyRB.linearVelocity.y);
                isAttacking = true;
                attackCanvas.SetActive(true);
                //after .75f set hitbox active
                switch (attackCooldownTimer)
                {
                    case < 0.75f:
                        break;
                    case < 2f:
                        enemyHitbox.SetActive(true);
                        break;
                    default:
                        enemyHitbox.SetActive(false);
                        attackCooldownTimer = 0;
                        break;
                }
                
            }
            else
            {
                attackCanvas.SetActive(false);
                isAttacking = false;
                enemyHitbox.SetActive(false);
                attackCooldownTimer = 0;
            }
        }

        Debug.Log(attackCooldownTimer);
    }
}
