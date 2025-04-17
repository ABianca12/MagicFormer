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

    [Header("Image UI Panels")]
    [SerializeField] public RawImage currImage;
    [SerializeField] public RawImage nxtImage;
    [SerializeField] public RawImage prevImage;

    private int currImgIndex = 0;
    private int nextImgIndex = 1;
    private int prevImgIndex = 4;
    public List<Texture2D> texList;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void cycleSpells(bool forward)
    {
        if(forward)
        {
            currImgIndex++;
            nextImgIndex++;
            prevImgIndex++;
        }
        else
        {
            currImgIndex--;
            nextImgIndex--;
            prevImgIndex--;
        }


        if (nextImgIndex >= texList.Count)
        {
            nextImgIndex = 0;
        }
        if (currImgIndex >= texList.Count)
        {
            currImgIndex = 0;
        }
        if (prevImgIndex >= texList.Count)
        {
            prevImgIndex = 0;
        }
        if (nextImgIndex < 0)
        {
            nextImgIndex = texList.Count - 1;
        }
        if (currImgIndex < 0)
        {
            currImgIndex = texList.Count - 1;
        }
        if (prevImgIndex < 0)
        {
            prevImgIndex = texList.Count - 1;
        }

        currImage.texture = texList[currImgIndex];
        nxtImage.texture = texList[nextImgIndex];
        prevImage.texture = texList[prevImgIndex];
    }
}
