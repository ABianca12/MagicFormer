using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public float scrollSpeed = 2;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * scrollSpeed);
    }
}
