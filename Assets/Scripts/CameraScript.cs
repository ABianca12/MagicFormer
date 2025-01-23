using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float zLock;
    [SerializeField] private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //updates camera position to follow the player at a constant z axis to maintain a 2-d perspective
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zLock);

    }
}
