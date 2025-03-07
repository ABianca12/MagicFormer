using TMPro;
using UnityEngine;

public class SwitchButton : Interactable
{
    public bool isPress = false;

    private bool independantSwitch;
    GameObject[] relatedBlocks;
    GameObject[] relatedSwitches;

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
            if (relatedBlocks[i].GetComponent<SwitchBlock>() != null)
            {
                relatedBlocks[i].GetComponent<SwitchBlock>().swapState();
            }
            else if(relatedBlocks[i].GetComponent<SwitchButton>() != null)
            {
                relatedBlocks[i].GetComponent<SwitchButton>().SwapStates();
            }
            
        }
        isPress = false;

    }

    public void SwapStates()
    {
        if (isActive)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }
    public void TurnOn()
    {
        isActive = true;
        switchText.text = "ON";
        gameObject.GetComponent<MeshRenderer>().material = onMat;
    }

    public void TurnOff()
    {
        isActive = false;
        switchText.text = "OFF";
        gameObject.GetComponent<MeshRenderer>().material = offMat;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fireball" || collision.gameObject.tag == "Player")
        {
            Press();
        }
    }
}
