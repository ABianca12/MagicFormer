using UnityEngine;
using System.Collections.Generic;

public class Destructable : Interactable
{
    //Init functions
    public void initDestructable()
    {
        base.initInteractable();
    }
    public void initDestructable(BaseType m)
    {
        base.initInteractable(m);
    }
    public void initDestructable(BaseType m, Vector3 start)
    {
        base.initInteractable(start, m);
    }

    public virtual void destroyObject()
    {
        Destroy(gameObject);
    }
}
