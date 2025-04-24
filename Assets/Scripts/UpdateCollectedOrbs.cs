using UnityEngine;
using UnityEngine.UI;

public class UpdateCollectedOrbs : MonoBehaviour
{
    public LevelData levelData;

    private OrbManager orbManager;

    public RawImage RedOrbSlot;
    public RawImage YellowOrbSlot;
    public RawImage BlueOrbSlot;

    private void Start()
    {
        orbManager = GameObject.FindWithTag("OrbManager").GetComponent<OrbManager>();
    }

    void Update()
    {
        if (levelData.HasCollectedRedOrb)
        {
            RedOrbSlot.GetComponent<RawImage>().texture = orbManager.CollectedRed;
        }
        else
        {
            RedOrbSlot.GetComponent<RawImage>().texture = orbManager.UncollectedOrb;
        }

        if (levelData.HasCollectedYellowOrb)
        {
            YellowOrbSlot.GetComponent<RawImage>().texture = orbManager.CollectedYellow;
        }
        else
        {
            YellowOrbSlot.GetComponent<RawImage>().texture = orbManager.UncollectedOrb;
        }

        if (levelData.HasCollectedBlueOrb)
        {
            BlueOrbSlot.GetComponent<RawImage>().texture = orbManager.CollectedBlue;
        }
        else
        {
            BlueOrbSlot.GetComponent<RawImage>().texture = orbManager.UncollectedOrb;
        }
    }
}
