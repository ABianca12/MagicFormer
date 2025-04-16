using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public static Canvas PauseMenu;

    [SerializeField]
    private static bool isPaused = false;

    private void Start()
    {
        PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public static void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        PauseMenu.enabled = isPaused;
    }

    public static bool GetIsPaused()
    {
        return isPaused;
    }
}
