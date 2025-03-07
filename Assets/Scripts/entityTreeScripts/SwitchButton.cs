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

    //on start gathers all switch blocks and switches
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

    //swaps states of all switch blocks and all switches
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

    //swaps text and material
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

    //changes active, text, and material
    public void TurnOn()
    {
        isActive = true;
        switchText.text = "ON";
        gameObject.GetComponent<MeshRenderer>().material = onMat;
    }

    //changes active, text, and material
    public void TurnOff()
    {
        isActive = false;
        switchText.text = "OFF";
        gameObject.GetComponent<MeshRenderer>().material = offMat;
    }

    //if collides with a fireball or player, trigger press
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fireball" || collision.gameObject.tag == "Player")
        {
            Press();
        }
    }
}
