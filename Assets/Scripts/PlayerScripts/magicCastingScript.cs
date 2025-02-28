using TarodevController;
using UnityEngine;
using static TarodevController.PlayerController;

public class magicCastingScript : MonoBehaviour
{
    //Spell prefabs
    [SerializeField] private Fireball f;
    [SerializeField] private Crate earfKrate;
    [SerializeField] private ForcePush poosh;
    private PlayerController p;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Casting fireball
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Debug.Log("FIRE:);
            Fireball fb = Instantiate(f);
            switch (p.getFaceDirection())
            {
                case PlayerDirection.Left:
                    fb.initFireball(gameObject.transform.position, new Vector2(-5, 0));
                    break;
                case PlayerDirection.Right:
                    fb.initFireball(gameObject.transform.position, new Vector2(5, 0));
                    break;
                default:
                    fb.initFireball(gameObject.transform.position, new Vector2(5, 0));
                    break;

            }

        }
        //Casting earth crate
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Debug.Log("EARTH");
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
        //Casting force push
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Debug.Log("FORCE BURST");
            ForcePush f = Instantiate(poosh);
            Vector3 point = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            var d = Input.mousePosition - point;
            d.Normalize();
            print(point);
            f.initForcePush(gameObject.transform.position, d * 10f);

        }

    }
}
