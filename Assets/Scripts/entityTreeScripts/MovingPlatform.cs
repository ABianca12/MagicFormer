using System.Linq;
using UnityEngine;

public class MovingPlatform : Ground
{
    [SerializeField] private Vector3 endPos = Vector3.zero;

    [SerializeField] private bool baseMoving;

    [SerializeField] private float speed;

    [SerializeField] private Material offMat, onMat;

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
            }
            else if (currentPos.x > endPos.x)
            {
                updateVelocity((new Vector2(1.0f, 0)) * -speed);
            }

        }
    }

    private void Update()
    {
        if(baseMoving)
        {
            if (currentPos.x >= endPos.x)
            {
                updateVelocity((new Vector2(1.0f, 0)) * -speed);
            }
            else if (currentPos.x <= startPos.x)
            {
                updateVelocity((new Vector2(1.0f, 0)) * speed);
            }

            updateEntity(Time.deltaTime);
        }

        checkMat();
    }

    private void checkMat()
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

}
