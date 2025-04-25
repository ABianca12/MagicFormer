using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public PauseSystem p;

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
