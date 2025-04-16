using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button Quit;

    private Scenemanager s;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        s = Object.FindFirstObjectByType<Scenemanager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
