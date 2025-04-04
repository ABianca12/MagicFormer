using TarodevController;
using UnityEngine;

public class Door : Ground
{
    private Scenemanager s;

    private void Start()
    {
        s = GameObject.FindWithTag("SceneManager").GetComponent<Scenemanager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().GetObjectBeingHeld() != null && collision.gameObject.GetComponent<PlayerController>().GetObjectBeingHeld().name == "Key")
            {
                //End level
                s.nextLevel();
            }
        }
    }
}
