using System.Linq;
using UnityEngine;

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

    public void PushPlat(Vector2 dir, float strength)
    {
        
    }

}
