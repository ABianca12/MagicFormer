using UnityEngine;

public class DataManager : MonoBehaviour
{
    public LevelData[] allLevelData;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public LevelData GetLevelData(LevelData levelData)
    {
        foreach (LevelData data in allLevelData)
        {
            if (data.name == levelData.name)
            {
                return data;
            }
        }

        return null;
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
