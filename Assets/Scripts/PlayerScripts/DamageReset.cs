using UnityEngine;

public class DamageReset : MonoBehaviour
{
    private Scenemanager s;

    private void Start()
    {
        s = GameObject.FindWithTag("SceneManager").GetComponent<Scenemanager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DamageBox")
        {
            s.restartLevel();
        }
        Debug.Log(collision.gameObject);
    }
}
