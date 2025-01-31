using UnityEngine;

public class MovingPlatform : Ground
{
    [SerializeField] private Vector3 endPos = Vector3.zero;

    [SerializeField] private bool baseMoving;

    [SerializeField] private float speed;

    public void initPlatform() { initGround(); }
    public void initPlatform(Vector3 startPos, Material m, Vector3 endingPos, bool startMove, float s)
    {
        initGround(startPos, m);
        endPos = endingPos;
        baseMoving = startMove;
        speed = s;
    }

    private void Update()
    {
        if (baseMoving) 
        {
            
        }
    }
}
