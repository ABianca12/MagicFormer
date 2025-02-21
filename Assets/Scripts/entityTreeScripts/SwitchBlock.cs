using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlock : Ground
{
    private bool activeState;

    [SerializeField] private Material offMat, onMat;

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

    public void turnOff()
    {
        activeState = false;
        gameObject.GetComponent<MeshRenderer>().material = offMat;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void turnOn()
    {
        activeState = true;
        gameObject.GetComponent<MeshRenderer>().material = onMat;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
