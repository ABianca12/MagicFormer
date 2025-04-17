using UnityEngine;
using UnityEngine.UI;

public class UpdateCollectedOrbs : MonoBehaviour
{
    public LevelData levelData;

    public Texture2D UncollectedOrb;
    public Texture2D CollectedRed;
    public Texture2D CollectedYellow;
    public Texture2D CollectedBlue;

    public RawImage RedOrbSlot;
    public RawImage YellowOrbSlot;
    public RawImage BlueOrbSlot;

    void Start()
    {
        if (levelData.HasCollectedRedOrb)
        {
            RedOrbSlot.GetComponent<RawImage>().texture = CollectedRed;
        }
        else
        {
            RedOrbSlot.GetComponent<RawImage>().texture = UncollectedOrb;
        }

        if (levelData.HasCollectedYellowOrb)
        {
            YellowOrbSlot.GetComponent<RawImage>().texture = CollectedYellow;
        }
        else
        {
            YellowOrbSlot.GetComponent<RawImage>().texture = UncollectedOrb;
        }

        if (levelData.HasCollectedBlueOrb)
        {
            BlueOrbSlot.GetComponent<RawImage>().texture = CollectedBlue;
        }
        else
        {
            BlueOrbSlot.GetComponent<RawImage>().texture = UncollectedOrb;
        }
    }
}
