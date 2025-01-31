using Unity.VisualScripting;
using UnityEngine;

public class SwitchBlock : Ground
{
    private UnityEngine.Material matOn;
    private UnityEngine.Material matOff;

    private Material currTex;

    bool activeState;

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
