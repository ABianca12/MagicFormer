using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    private void Start()
    {
        //pauseSystem = Object.FindFirstObjectByType<PauseSystem>();
    }

    public void LoadLevel(string levelString)
    {
        if (PauseSystem.GetIsPaused())
        {
            PauseSystem.PauseGame();
        }

        SceneManager.LoadScene(levelString);
    }
}
