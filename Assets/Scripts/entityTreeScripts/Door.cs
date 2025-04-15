using TarodevController;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Ground
{
    public LevelData levelData;
    private Scenemanager s;

    private void Start()
    {
        s = GameObject.FindWithTag("SceneManager").GetComponent<Scenemanager>();
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
                        levelData.HasCollectedRedOrb = true;
                    }
                    else if (obj.CompareTag("YellowOrb"))
                    {
                        levelData.HasCollectedYellowOrb = true;
                    }
                    else if (obj.CompareTag("BlueOrb"))
                    {
                        levelData.HasCollectedBlueOrb = true;
                    }
                }

                SceneManager.LoadScene("LevelSelect");
            }
        }
    }
}
