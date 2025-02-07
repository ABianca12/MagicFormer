using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlock : Ground
{
    private UnityEngine.Material matOn;
    private UnityEngine.Material matOff;

    private Material currTex;

    bool activeState;


    public void swapState()
    {
        if (activeState)
        {
            activeState = false;
            setMaterial(Material.None);
            gameObject.GetComponent<MeshRenderer>().material = matOn;
        }
        else
        {
            activeState = true;
            setMaterial(Material.Metal);
            gameObject.GetComponent<MeshRenderer>().material = matOff;
        }

    }

    public void turnOff()
    {
        activeState = false;
        setMaterial(Material.None);
        gameObject.GetComponent<MeshRenderer>().material = matOn;
    }

    public void turnOn()
    {
        activeState = true;
        setMaterial(Material.Metal);
        gameObject.GetComponent<MeshRenderer>().material = matOff;
    }
}
