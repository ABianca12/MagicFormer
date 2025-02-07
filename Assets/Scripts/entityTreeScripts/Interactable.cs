using UnityEngine;

public class Interactable : Entity
{
    public void initInteractable()
    {
        initEntity();
    }
    public void initInteractable(Vector3 start, Vector2 v, Material m)
    {
        initEntity(start, v, m);
    }
    public void initInteractable(Vector3 start)
    {
        initEntity(start);
    }
    public void initInteractable(Material m)
    {
        initEntity(m);
    }
    public void initInteractable(Vector3 sPos, Material m)
    {
        initEntity(sPos, m);
    }
    public void initInteractable(Vector3 start, Vector2 v)
    {
        initEntity(start, v);
    }
}
