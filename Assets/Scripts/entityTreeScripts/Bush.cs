using UnityEngine;

public class Bush : Destructable
{
    public void initBush()
    {
        base.initDestructable(BaseType.GRASS);
    }
    public void initBush(Vector3 start)
    {
        base.initDestructable(BaseType.GRASS, start);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initBush(gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
