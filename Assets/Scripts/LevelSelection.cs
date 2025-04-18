using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public PauseSystem p;
    private void Start()
    {
        //p = GameObject.FindWithTag("PauseManager").GetComponent<PauseSystem>();
    }

    public void LoadLevel(string levelString)
    {
        if (p.GetIsPaused())
        {
            p.PauseGame();
        }

        SceneManager.LoadScene(levelString);
    }

    public void LoadLevelByName(string levelString)
    {
        SceneManager.LoadScene(levelString);
    }
}
