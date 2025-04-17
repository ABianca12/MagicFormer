using UnityEngine;

public class ParticleSystemScript : MonoBehaviour
{
    float totalDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalDuration = gameObject.GetComponent<ParticleSystem>().duration + gameObject.GetComponent<ParticleSystem>().startLifetime;
        Destroy(gameObject, totalDuration);
    }
    private void Update()
    {
        
    }
}
