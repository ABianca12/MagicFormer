using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private PauseSystem p;
    private void Start()
    {
        //p = Object.FindFirstObjectByType<PauseSystem>();
    }

    public void LoadLevel(string levelString)
    {
        //if (p.GetIsPaused())
        //{
        //    p.PauseGame();
        //}

        SceneManager.LoadScene(levelString);
    }
}
