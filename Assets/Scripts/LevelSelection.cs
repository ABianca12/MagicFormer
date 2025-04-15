using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public void LoadLevel(string levelString)
    {
        SceneManager.LoadScene(levelString);
    }
}
