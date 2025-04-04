using UnityEngine;

public class RestartGame : MonoBehaviour
{
    private Scenemanager sm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<Scenemanager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            sm.loadLevel(0);
        }
    }
}
