using UnityEngine;

public class LoadLevelData : MonoBehaviour
{
    public LevelData levelData;
    private Inventory inventory;
    private bool[] spells = new bool[5];

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        if (levelData.PlayerStartsWithFireball)
        {
            spells[0] = true;
        }
        else
        {
            spells[0] = false;
        }

        if (levelData.PlayerStartsWithEarthCrate)
        {
            spells[1] = true;
        }
        else
        {
            spells[1] = false;
        }

        if (levelData.PlayerStartsWithForcePush)
        {
            spells[2] = true;
        }
        else
        {
            spells[2] = false;
        }

        if (levelData.PlayerStartsWithVine)
        {
            spells[3] = true;
        }
        else
        {
            spells[3] = false;
        }

        if (levelData.PlayerStartsWithTimestop)
        {
            spells[4] = true;
        }
        else
        {
            spells[4] = false;
        }

        inventory.initInventory(spells);
    }

    private void Update()
    {
        
    }
}
