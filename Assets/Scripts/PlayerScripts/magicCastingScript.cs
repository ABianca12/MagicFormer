using TarodevController;
using UnityEngine;
using static TarodevController.PlayerController;

public class magicCastingScript : MonoBehaviour
{
    //Spell prefabs
    [SerializeField] private Fireball f;
    [SerializeField] private Crate earfKrate;
    private PlayerController p;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Fireball fb = Instantiate(f);
            switch(p.getFaceDirection())
            {
                case PlayerDirection.Left:
                    fb.initFireball(gameObject.transform.position,new Vector2(-5, 0));
                    break;
                case PlayerDirection.Right:
                    fb.initFireball(gameObject.transform.position, new Vector2(5, 0));
                    break;
                default:
                    fb.initFireball(gameObject.transform.position, new Vector2(5, 0));
                    break;

            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("EARTH");
            Crate c = Instantiate(earfKrate);
            switch (p.getFaceDirection())
            {
                case PlayerDirection.Left:
                    c.initCrate(new Vector3(gameObject.transform.position.x - 2, gameObject.transform.position.y, gameObject.transform.position.z));
                    break;
                case PlayerDirection.Right:
                    c.initCrate(new Vector3(gameObject.transform.position.x + 2, gameObject.transform.position.y, gameObject.transform.position.z));
                    break;
                default:
                    c.initCrate(new Vector3(gameObject.transform.position.x + 2, gameObject.transform.position.y, gameObject.transform.position.z));
                    break;

            }
        }
    }
}
