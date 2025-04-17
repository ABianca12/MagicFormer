using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public Canvas PauseMenu;

    private bool isPaused = false;

    private void Start()
    {
        PauseMenu = GameObject.FindWithTag("PauseMenu").GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        PauseMenu.enabled = isPaused;
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }
}
