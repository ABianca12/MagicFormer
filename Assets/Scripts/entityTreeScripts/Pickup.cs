using System;
using TarodevController;
using UnityEngine;
using static TarodevController.PlayerController;

public class Pickup : Destructable
{

    public void initPickup()
    {
        base.initDestructable();
    }
    public void initPickup(BaseType m)
    {
        base.initDestructable(m);
    }
    public void initPickup(BaseType m, Vector3 start)
    {
        base.initDestructable(m, start);
    }

}
