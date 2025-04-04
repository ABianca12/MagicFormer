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

    [Header("Image UI Panels")]
    [SerializeField] public RawImage currImage;
    [SerializeField] public RawImage nxtImage;

    private int currImgIndex = 0;
    private int nextImgIndex = 1;
    public List<Texture2D> texList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texList.Add(Fireball);
        texList.Add(Crate);
        texList.Add(Wind);
        texList.Add(Vine);

        currImage.texture = texList[currImgIndex];
        nxtImage.texture = texList[nextImgIndex];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void cycleSpells()
    {
        currImgIndex++;
        nextImgIndex++;

        if (nextImgIndex >= texList.Count)
        {
            nextImgIndex = 0;
        }
        if (currImgIndex >= texList.Count)
        {
            currImgIndex = 0;
        }

        currImage.texture = texList[currImgIndex];
        nxtImage.texture = texList[nextImgIndex];
    }
}
