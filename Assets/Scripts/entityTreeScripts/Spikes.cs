using UnityEngine;
using UnityEngine.Timeline;

public class Spikes : Ground
{
    private Scenemanager s;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        s = GameObject.FindWithTag("SceneManager").GetComponent<Scenemanager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            s.restartLevel();
        }
    }
}
