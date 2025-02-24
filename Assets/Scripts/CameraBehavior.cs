using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject player;
    public Vector3 PlayingCameraOffset;
    public Quaternion PlayingCamAngle;

    private bool mFollowPlayer = true;

    void Start()
    {
        transform.position = new Vector3(player.transform.position.x + PlayingCameraOffset.x,
                player.transform.position.y + PlayingCameraOffset.y,
                player.transform.position.z + PlayingCameraOffset.z);
        transform.rotation = PlayingCamAngle;
    }

    void Update()
    {
        if (mFollowPlayer)
        {
            transform.position = new Vector3(player.transform.position.x + PlayingCameraOffset.x,
                player.transform.position.y + PlayingCameraOffset.y,
                player.transform.position.z + PlayingCameraOffset.z);
            transform.rotation = PlayingCamAngle;
        }
    }

    public void SetFollowPlayer(bool followPlayer)
    {
        mFollowPlayer = followPlayer;
    }

    public bool GetFollowPlayer()
    {
        return mFollowPlayer;
    }
}
