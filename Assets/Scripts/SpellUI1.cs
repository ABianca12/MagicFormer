using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
    [Header("Images for spells")]
    [SerializeField] private Texture2D Fireball;
    [SerializeField] private Texture2D Crate;
    [SerializeField] private Texture2D Wind;
    [SerializeField] private Texture2D Vine;
    [SerializeField] private Texture2D Timestop;
    [SerializeField] private Texture2D None;

    [Header("Image UI Panels")]
    [SerializeField] public RawImage currImage;
    [SerializeField] public RawImage nxtImage;
    [SerializeField] public RawImage prevImage;

    private int currImgIndex = 0;
    private int nextImgIndex = 1;
    private int prevImgIndex = 4;
    public List<Texture2D> texList;
    private Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texList.Add(Fireball);
        texList.Add(Crate);
        texList.Add(Wind);
        texList.Add(Vine);
        texList.Add(Timestop);

        prevImgIndex = texList.Count - 1;
        currImage.texture = texList[currImgIndex];
        nxtImage.texture = texList[nextImgIndex];
        prevImage.texture = texList[prevImgIndex];
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        cycleSpells();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void cycleSpells()
    {
        int old = currImgIndex;
        //Active spell image
        currImgIndex = inventory.getCurrentItem();

        //Next spell image
        nextImgIndex = currImgIndex + 1;
        if (nextImgIndex < 0)
        {
            nextImgIndex = texList.Count - 1;
        }
        if (nextImgIndex >= texList.Count)
        {
            nextImgIndex = 0;
        }
        old = nextImgIndex;
        //loops around until it finds the next spell that the player owns
        while (!inventory.hasItem(nextImgIndex))
        {
            nextImgIndex = nextImgIndex++;
            if (nextImgIndex < 0)
            {
                nextImgIndex = texList.Count - 1;
            }
            if (nextImgIndex >= texList.Count)
            {
                nextImgIndex = 0;
            }
            if(nextImgIndex == old)
            {
                nextImgIndex = -1;
                break;
            }
        }

        //previous spell image
        prevImgIndex = currImgIndex - 1;
        if (prevImgIndex >= texList.Count)
        {
            prevImgIndex = 0;
        }
        if (prevImgIndex < 0)
        {
            prevImgIndex = texList.Count - 1;
        }
        old = prevImgIndex;
        //loops around until it finds the previous spell the player owns
        while(!inventory.hasItem(prevImgIndex))
        {
            prevImgIndex = prevImgIndex--;
            if (prevImgIndex >= texList.Count)
            {
                prevImgIndex = 0;
            }
            if (prevImgIndex < 0)
            {
                prevImgIndex = texList.Count - 1;
            }
            if (prevImgIndex == old)
            {
                prevImgIndex = -1;
                break;
            }
        }

        //Debug.Log("current " + currImgIndex);
        //Debug.Log("next " + nextImgIndex);
        //Debug.Log(prevImgIndex);

        //Conditionals for setting default texture if the player doesn't have a current spell they can activate
        if(currImgIndex < 0)
        {
            currImage.texture = None;
        }
        else
        {
            currImage.texture = texList[currImgIndex];
        }
        //For next image
        if (nextImgIndex < 0)
        {
            nxtImage.texture = None;
        }
        else
        {
            nxtImage.texture = texList[nextImgIndex];
        }
        if(prevImgIndex < 0)
        {
            prevImage.texture = None;
        }
        else
        {
            prevImage.texture = texList[prevImgIndex];
        }

    }
}
