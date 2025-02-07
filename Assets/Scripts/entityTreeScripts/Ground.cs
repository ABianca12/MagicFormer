using UnityEngine;
using System.Collections.Generic;
public class Ground : Entity
{
    public void initGround(Vector3 start, Material m)
    {
        initEntity(start, m);
    }
    public void initGround(Vector3 start, Vector2 v, Material m)
    {
        initEntity(start, v, m);
    }
    public void initGround(Vector3 start)
    {
        initEntity(start);
    }
    public void initGround(Material m)
    {
        initEntity(m);
    }
    public void initGround(Vector3 start, Vector2 v)
    {
        initEntity(start, v);
    }
    public void initGround()
    {
        initEntity();
    }
}
