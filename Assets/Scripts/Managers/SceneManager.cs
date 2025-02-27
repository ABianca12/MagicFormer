using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene("NewMovementTesting");
    }
}
