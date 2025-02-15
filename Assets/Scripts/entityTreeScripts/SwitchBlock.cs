using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlock : Ground
{
    private UnityEngine.Material matOn;
    private UnityEngine.Material matOff;

    private BaseType currTex;

    bool activeState;


    public void swapState()
    {
        if (activeState)
        {
            activeState = false;
            setMaterial(BaseType.NONE);
            gameObject.GetComponent<MeshRenderer>().material = matOn;
        }
        else
        {
            activeState = true;
            setMaterial(BaseType.METAL);
            gameObject.GetComponent<MeshRenderer>().material = matOff;
        }

    }

    public void turnOff()
    {
        activeState = false;
        setMaterial(BaseType.NONE);
        gameObject.GetComponent<MeshRenderer>().material = matOn;
    }

    public void turnOn()
    {
        activeState = true;
        setMaterial(BaseType.METAL);
        gameObject.GetComponent<MeshRenderer>().material = matOff;
    }
}
