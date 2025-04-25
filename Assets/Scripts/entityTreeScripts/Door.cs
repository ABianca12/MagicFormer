using TarodevController;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Ground
{
    public LevelData levelData;
    private LevelData FoundLevelData;
    public DataManager dataManager;
    private Scenemanager s;

    private void Start()
    {
        s = GameObject.FindWithTag("SceneManager").GetComponent<Scenemanager>();
        dataManager = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
        FoundLevelData = dataManager.GetLevelData(levelData);
        Debug.Log(levelData.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().GetObjectBeingHeld() != null && collision.gameObject.GetComponent<PlayerController>().GetObjectBeingHeld().name == "Key")
            {
                foreach (var obj in collision.gameObject.GetComponent<Inventory>().collectables)
                {
                    if (obj.CompareTag("RedOrb"))
                    {
                        FoundLevelData.HasCollectedRedOrb = true;
                    }
                    else if (obj.CompareTag("YellowOrb"))
                    {
                        FoundLevelData.HasCollectedYellowOrb = true;
                    }
                    else if (obj.CompareTag("BlueOrb"))
                    {
                        FoundLevelData.HasCollectedBlueOrb = true;
                    }
                }

                SceneManager.LoadScene("LevelSelect");
            }
        }
    }
}
