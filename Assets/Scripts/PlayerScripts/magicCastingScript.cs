using UnityEngine;

public class magicCastingScript : MonoBehaviour
{
    //Spell prefabs
    [SerializeField] private Fireball f;
    [SerializeField] private Crate earfKrate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Fireball fb = Instantiate(f);
            fb.initFireball(gameObject.transform.position, new Vector2(5,0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("EARTH");
            Crate c = Instantiate(earfKrate);
            c.initCrate(gameObject.transform.position);
            //c.initCrate(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 2, gameObject.transform.position.z));
            //Call player pickup function to make them pick up the crate
        }
    }
}
