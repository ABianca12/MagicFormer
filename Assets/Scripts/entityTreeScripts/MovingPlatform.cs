using System.Linq;
using TarodevController;
using UnityEngine;
using UnityEngine.Diagnostics;

public class MovingPlatform : Ground
{
    [SerializeField] private Vector3 endPos = Vector3.zero;

    [SerializeField] private bool baseMoving;

    [SerializeField] private float speed;

    [SerializeField] private Material offMat, onMat;

    [SerializeField] private PlayerController player;
    [SerializeField] private bool startUpOnY = true;

    private bool disableX;
    private bool playerIsOnTop;

    public void initPlatform() { initGround(); }
    public void initPlatform(Vector3 startPos, BaseType m, Vector3 endingPos, bool startMove, float s)
    {
        initGround(startPos, m);
        endPos = endingPos;
        baseMoving = startMove;
        speed = s;
    }

    //makes a platform and sets its velocity based on public variables
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        initPlatform(gameObject.transform.position, material, endPos, baseMoving, speed);

        if (baseMoving)
        {
            if (currentPos.x < endPos.x)
            {
                updateVelocity((new Vector2(1.0f, 0)) * speed);
                disableX = false;
            }
            else if (currentPos.x > endPos.x)
            {
                updateVelocity((new Vector2(1.0f, 0)) * -speed);
                disableX = false;
            }

            else if (currentPos.y < endPos.y)
            {
                updateVelocity((new Vector2(0, 1.0f)) * speed);
                disableX = true;
            }
            else if (currentPos.y > endPos.y)
            {
                updateVelocity((new Vector2(0, 1.0f)) * -speed);
                disableX = true;
            }
        }
        else
        {
            if (currentPos.x < endPos.x)
            {
                disableX = false;
            }
            else if (currentPos.x > endPos.x)
            {
                disableX = false;
            }

            else if (currentPos.y < endPos.y)
            {
                disableX = true;
            }
            else if (currentPos.y > endPos.y)
            {
                disableX = true;
            }
        }
        
    }

    //updates velocity and swaps velocity once a platform reaches its endpoint
    private void Update()
    {
        if(baseMoving)
        {
            //If platform is meant to move
            if (!disableX)
            {
                if (currentPos.x > endPos.x)
                {
                    currentPos.x = endPos.x;
                    transform.position = currentPos;
                    updateVelocity((new Vector2(1.0f, 0)) * -speed);
                }
                else if (currentPos.x < startPos.x)
                {
                    currentPos.x = startPos.x;
                    transform.position = currentPos;
                    updateVelocity((new Vector2(1.0f, 0)) * speed);
                }
            }
            else
            {
                if (currentPos.y < endPos.y)
                {
                    currentPos.y = endPos.y;
                    transform.position = currentPos;
                    updateVelocity((new Vector2(0, 1.0f)) * speed);
                }
                else if (currentPos.y > startPos.y)
                {
                    currentPos.y = startPos.y;
                    transform.position = currentPos;
                    updateVelocity((new Vector2(0, 1.0f)) * -speed);
                }
            }
        }
        //if platform is standing still
        else
        {
            if (!disableX)
            {
                if (currentPos.x > endPos.x)
                {
                    currentPos.x = endPos.x;
                    transform.position = currentPos;
                    updateVelocity((new Vector2(-0.8f, 0)) * velocity);
                }
                else if (currentPos.x < startPos.x)
                {
                    currentPos.x = startPos.x;
                    transform.position = currentPos;
                    updateVelocity((new Vector2(-0.8f, 0)) * velocity);
                }
            }
            else
            {
                if(startUpOnY)
                {
                    if (currentPos.y < endPos.y)
                    {
                        currentPos.y = endPos.y;
                        transform.position = currentPos;
                        updateVelocity((new Vector2(0, -0.8f)) * velocity);
                    }
                    else if (currentPos.y > startPos.y)
                    {
                        currentPos.y = startPos.y;
                        transform.position = currentPos;
                        updateVelocity((new Vector2(0, -0.8f)) * velocity);
                    }
                }
                else
                {
                    if (currentPos.y > endPos.y)
                    {
                        currentPos.y = endPos.y;
                        transform.position = currentPos;
                        updateVelocity((new Vector2(0, -0.8f)) * velocity);
                    }
                    else if (currentPos.y < startPos.y)
                    {
                        currentPos.y = startPos.y;
                        transform.position = currentPos;
                        updateVelocity((new Vector2(0, -0.8f)) * velocity);
                    }
                }
            }
        }

        updateEntity(Time.deltaTime);
        CheckMat();
    }

    //swaps a switch platforms material based on if its on/off
    private void CheckMat()
    {
        if (baseMoving) 
        {
            gameObject.GetComponent<MeshRenderer>().material = onMat;
        }
        else if(!baseMoving)
        {
            gameObject.GetComponent<MeshRenderer>().material = offMat;
        }
    }

    //adds force when encounters Force Push spell
    public override void addForce(Vector2 force, float strength = 2.0f)
    {
        //If platform is moving along the x axis
        if(!disableX)
        {
            base.velocity += new Vector2(force.x, 0);
        }
        else
        {
            base.velocity += new Vector2(0,force.y);
        }
    }

    //binds players movement to set platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform, true);
        }
        else if (collision.gameObject.CompareTag("PickUp"))
        {
            collision.gameObject.transform.SetParent(transform, true);
        }
    }

    //unbinds players movement
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.CompareTag("PickUp"))
        {
            transform.DetachChildren();
        }
    }
}
