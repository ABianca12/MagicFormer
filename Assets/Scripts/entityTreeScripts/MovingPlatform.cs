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

    private bool disableX;

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

            if (currentPos.y < endPos.y)
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
    }

    //updates velocity and swaps velocity once a platform reaches its endpoint
    private void Update()
    {
        if(baseMoving)
        {
            if (!disableX)
            {
                if (currentPos.x > endPos.x)
                {
                    updateVelocity((new Vector2(1.0f, 0)) * -speed);
                }
                else if (currentPos.x < startPos.x)
                {
                    updateVelocity((new Vector2(1.0f, 0)) * speed);
                }
            }
            else
            {
                if (currentPos.y < endPos.y)
                {
                   updateVelocity((new Vector2(0, 1.0f)) * speed);
                }
                else if (currentPos.y > startPos.y)
                {
                    updateVelocity((new Vector2(0, 1.0f)) * -speed);
                }
            }

            updateEntity(Time.deltaTime);
        }

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
            base.velocity += new Vector2(force.x * strength, 0);
        }
        else
        {
            base.velocity += new Vector2(0,force.y * strength);
        }
    }

    //binds players movement to set platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform, true);
        }
    }

    //unbinds players movement
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.DetachChildren();
        }
    }
}
