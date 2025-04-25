using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float zLock;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 distanceToPlayer;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x + distanceToPlayer.x, player.transform.position.y + distanceToPlayer.y, zLock);
    }
}
