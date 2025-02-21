using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchButton : Interactable
{

    private bool independantSwitch;
    GameObject[] relatedBlocks;

    public bool isPress = false;

    private bool isActive = false;

    private TextMeshPro switchText;

    [SerializeField] private Material offMat, onMat;

    private void Start()
    {
        relatedBlocks = GameObject.FindGameObjectsWithTag("SwitchBlock");
        switchText = gameObject.GetComponentInChildren<TextMeshPro>();

    }
    private void Update()
    {
        if (isPress)
        {
            Press();
        }

    }

    private void Press()
    {
        for (int i = 0; i < relatedBlocks.Length; i++)
        {
            relatedBlocks[i].GetComponent<SwitchBlock>().swapState();
        }
        isPress = false;

        if (isActive)
        {
            isActive = false;
            switchText.text = "OFF";
            gameObject.GetComponent<MeshRenderer>().material = offMat;
        }
        else
        {
            isActive = true;
            switchText.text = "ON";
            gameObject.GetComponent<MeshRenderer>().material = onMat;
        }


    }
}
