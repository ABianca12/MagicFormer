using TarodevController;
using UnityEngine;
using static TarodevController.PlayerController;

public class magicCastingScript : MonoBehaviour
{
    [Header("UI Canvas")]
    [SerializeField] private SpellUI spellUI;

    //Spell prefabs
    [SerializeField] private Fireball fireball;
    [SerializeField] private Crate earfKrate;
    [SerializeField] private ForcePush poosh;
    [SerializeField] private Climbable vine;
    private PlayerController p;
    private Inventory inventory;
    private float planeDistance;
    private Plane nearPlane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p = gameObject.GetComponent<PlayerController>();
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        bool[] temp = { true, true, true, true, false };
        inventory.initInventory(temp);
        planeDistance = 0.3f;
        nearPlane = new Plane(Vector3.forward, planeDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            switch(inventory.getCurrentItem())
            {
                //Casting fireball
                case 0:
                    //Debug.Log("FIRE:);
                    Fireball fb = Instantiate(fireball);
                    switch (p.getFaceDirection())
                    {
                        case PlayerDirection.Left:
                            fb.initFireball(gameObject.transform.position - new Vector3(2, 0), new Vector2(-20, 0));
                            break;
                        case PlayerDirection.Right:
                            fb.initFireball(gameObject.transform.position + new Vector3(2, 0), new Vector2(20, 0));
                            break;
                        default:
                            fb.initFireball(gameObject.transform.position + new Vector3(2, 0), new Vector2(20, 0));
                            break;

                    }
                break;
                //Casting earth crate
                case 1:
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
                break;
                //Casting Force push
                case 2:
                    //Debug.Log("FORCE BURST");
                    ForcePush f = Instantiate(poosh);
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    var d = Input.mousePosition - screenPoint;
                    d.Normalize();
                    f.initForcePush(gameObject.transform.position, d * 20f);
                break;
                //Casting Vines
                case 3:
                    //Debug.Log("VINE TIME");
                    Climbable v = Instantiate(vine);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float enter = 0.0f;
                    //Raycast to the near plane on the camera
                    if(nearPlane.Raycast(ray, out enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);
                        v.initClimbable(Entity.BaseType.GRASS, new Vector3(hitPoint.x, hitPoint.y, transform.position.z));
                    }
                break;
                //Casting Timestop
                case 4:
                    //Debug.Log("ZA WARUDOOOOOOO");

                break;
                //Default is fireball
                default:
                    //Debug.Log("FIRE:);
                    Fireball fi = Instantiate(fireball);
                    switch (p.getFaceDirection())
                    {
                        case PlayerDirection.Left:
                            fi.initFireball(gameObject.transform.position - new Vector3(2, 0), new Vector2(-20, 0));
                            break;
                        case PlayerDirection.Right:
                            fi.initFireball(gameObject.transform.position + new Vector3(2, 0), new Vector2(20, 0));
                            break;
                        default:
                            fi.initFireball(gameObject.transform.position + new Vector3(2, 0), new Vector2(20, 0));
                            break;

                    }
                    break;
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            inventory.prevItem();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.nextItem();
            spellUI.cycleSpells();
        }

    }

}
