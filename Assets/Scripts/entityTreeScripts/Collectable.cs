using UnityEngine;
using System.Collections.Generic;

public class Collectable : Interactable
{
    //Member Variables
    protected Inventory pInventory;

    //init functions
    public void initCollectable()
    {
        initInteractable();
        pInventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
    }
    public void initCollectable(Vector3 start)
    {
        initInteractable(start);
        pInventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        //if Player collides with the object
        if(c.gameObject.tag == "Player")
        {
            //Collect Item

            Destroy(gameObject);
        }
    }

}
