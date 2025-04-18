using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    private bool[] allItems = new bool[5];
    private int currentItem = 0;
    public List<GameObject> collectables;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectables = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function to preset what the player has in their inventory.  Best used when loading a level
    public void initInventory(bool[] init)
    {
        allItems = init;
        foreach (bool item in allItems)
        {
            //Debug.Log(item);
        }
    }

    //Functions for current inventory slot
    //Function to get the index of the currently selected item
    public int getCurrentItem()
    {
        foreach (bool b in allItems)
        {
            if(b)
            {
                return currentItem;
            }
        }
        return -1;
    }
    //function to go to next available item in inventory that the player has
    public void nextItem()
    {
        int start = currentItem;
        currentItem++;
        if(currentItem > 4)
        {
            currentItem = 0;
        }
        //Debug.Log("CURRENT: " + currentItem + " " + allItems[currentItem]);
        while (allItems[currentItem] == false)
        {
            currentItem++;
            if (currentItem > 4)
            {
                currentItem = 0;
            }
            if(currentItem == start)
            {
                break;
            }
        }
    }
    //Funcion to go to previous available item in inventory that player has
    public void prevItem()
    {
        int start = currentItem;
        currentItem--;
        if (currentItem < 0)
        {
            currentItem = 4;
        }
        //Debug.Log("CURRENT: " + currentItem + " " + allItems[currentItem]);
        while (allItems[currentItem] == false)
        {
            currentItem--;
            if (currentItem < 0)
            {
                currentItem = 4;
            }
            if (currentItem == start)
            {
                break;
            }
        }
    }

    //functions for adding items
    public void addCollectable(GameObject g)
    {
        collectables.Add(g);
    }
    //Function for adding spells.  Pass in the index for the spell you want to add (0=fireball, 1=crate, 2=push, 3=vine, 4=timestop)
    public void addSpell(int spell)
    {
        if (!allItems[spell])
        {
            allItems[spell] = true;
        }
    }

    //Unique getter functions
    public bool getFire()
    {
        return allItems[0];
    }
    public bool getCrate()
    {
        return allItems[1];
    }
    public bool getPush()
    {
        return allItems[2];
    }
    public bool getVine()
    {
        return allItems[3];
    }
    public bool getTimestop()
    {
        return allItems[4];
    }

}
