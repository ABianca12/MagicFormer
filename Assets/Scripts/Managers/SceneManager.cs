using TarodevController;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    private AudioSource bwomp;
    private static bool bwompReady;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        bwomp = GetComponent<AudioSource>();

        if (bwompReady)
        {
            bwomp.Play();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            restartLevel();
        }

    }
    public void restartLevel()
    {
        bwompReady = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void nextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void loadLevelByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
