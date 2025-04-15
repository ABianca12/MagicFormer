using UnityEngine;
using System.Collections.Generic;

public class Collectable : Interactable
{
    //Member Variables
    protected Inventory pInventory;

    //init functions
    public void initCollectable()
    {
        pInventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        initInteractable();
    }
    public void initCollectable(Vector3 start)
    {
        pInventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        initInteractable(start);
    }
    public void initCollectable(Vector3 start, BaseType m)
    {
        pInventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        initInteractable(start, m);
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        Debug.Log("COLLUDE");
        //if Player collides with the object
        if(c.gameObject.tag == "Player")
        {
            //Collect Item
            c.gameObject.GetComponent<Inventory>().addCollectable(gameObject);

            this.gameObject.GetComponent<Renderer>().enabled = false;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

}
