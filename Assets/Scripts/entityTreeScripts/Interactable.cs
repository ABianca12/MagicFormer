using UnityEngine;

public class Interactable : Entity
{
    //init functions
    public void initInteractable()
    {
        initEntity();
    }
    public void initInteractable(Vector3 start, Vector2 v, BaseType m)
    {
        initEntity(start, v, m);
    }
    public void initInteractable(Vector3 start)
    {
        initEntity(start);
    }
    public void initInteractable(BaseType m)
    {
        initEntity(m);
    }
    public void initInteractable(Vector3 sPos, BaseType m)
    {
        initEntity(sPos, m);
    }
    public void initInteractable(Vector3 start, Vector2 v)
    {
        initEntity(start, v);
    }
}
