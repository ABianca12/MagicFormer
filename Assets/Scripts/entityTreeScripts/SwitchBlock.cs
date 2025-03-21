using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlock : Ground
{
    private bool activeState;

    [SerializeField] private Material offMat, onMat;

    private void Start()
    {
        if (!activeState)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //swaps block to differnet material and swaps its collision
    public void swapState()
    {
        if (activeState)
        {
            turnOff();
        }
        else
        {
            turnOn();
        }
    }

    //disables collision and swaps material of active block
    public void turnOff()
    {
        activeState = false;
        gameObject.GetComponent<MeshRenderer>().material = offMat;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    //enables collision and swaps material of active block
    public void turnOn()
    {
        activeState = true;
        gameObject.GetComponent<MeshRenderer>().material = onMat;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
