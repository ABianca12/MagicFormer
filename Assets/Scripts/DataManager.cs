using UnityEngine;

public class DataManager : MonoBehaviour
{
    public LevelData[] allLevelData;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetData()
    {
        foreach (LevelData data in allLevelData)
        {
            data.HasCollectedRedOrb = false;
            data.HasCollectedYellowOrb = false;
            data.HasCollectedBlueOrb = false;
        }
    }
}
