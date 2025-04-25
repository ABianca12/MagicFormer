using TarodevController;
using UnityEngine;
using static TarodevController.PlayerController;
using System.Collections.Generic;

public class magicCastingScript : MonoBehaviour
{
    [Header("UI Canvas")]
    [SerializeField] private SpellUI spellUI;

    //Spell prefabs
    [SerializeField] private Fireball fireball;
    [SerializeField] private Crate earfKrate;
    [SerializeField] private int maxCrates = 5;
    [SerializeField] private ForcePush poosh;
    [SerializeField] private Climbable vine;
    [SerializeField] private int maxVines = 5;
    [SerializeField] private Timestop sigil;

    private PlayerController p;
    private Inventory inventory;
    private float planeDistance;
    private Plane nearPlane;
    private bool sigilOut = false;
    private Timestop outStop;
    private int currVines = 0;
    private Queue<Crate> spawnedCrates;
    private Queue<Climbable> spawnedVines;

    private AudioSource[] allAudios;
    [SerializeField] private PauseSystem pause;
    private AudioSource fireSound;
    private AudioSource earthSound;
    private AudioSource pushSound;
    private AudioSource vineSound;
    private AudioSource timestopSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p = gameObject.GetComponent<PlayerController>();
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        planeDistance = 0.3f;
        nearPlane = new Plane(Vector3.forward, planeDistance);
        spellUI = GameObject.FindWithTag("SpellUI").GetComponent<SpellUI>();
        pause = GameObject.FindWithTag("PauseManager").GetComponent<PauseSystem>();
        spawnedCrates = new Queue<Crate>();
        spawnedVines = new Queue<Climbable>();
        allAudios = GetComponents<AudioSource>();

        foreach (AudioSource audio in allAudios)
        {
            if (audio.resource.name == "fireball")
            {
                fireSound = audio;
            }
            if(audio.resource.name == "Earth")
            {
                earthSound = audio;
            }
            if(audio.resource.name == "ForcePush")
            {
                pushSound = audio;
            }
            if (audio.resource.name == "Vine")
            {
                vineSound = audio;
            }
            if (audio.resource.name == "Timestop")
            {
                timestopSound = audio;
            }
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!pause.GetIsPaused())
            {
                switch (inventory.getCurrentItem())
                {
                    //Casting fireball
                    case 0:
                        //Debug.Log("FIRE:);
                        Fireball fb = Instantiate(fireball);
                        fireSound.Stop();
                        fireSound.Play();
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
                        Vector3 leftFace = new Vector3(gameObject.transform.position.x - 2.5f, gameObject.transform.position.y, gameObject.transform.position.z);
                        Vector3 rightFace = new Vector3(gameObject.transform.position.x + 2.5f, gameObject.transform.position.y, gameObject.transform.position.z);
                        LayerMask crate = LayerMask.GetMask("PickUp");
                        Crate c = Instantiate(earfKrate);
                        earthSound.Stop();
                        switch (p.getFaceDirection())
                        {
                            case PlayerDirection.Left:
                                if(Physics2D.Raycast(transform.position, transform.position - rightFace, 2.5f, crate))
                                {
                                    //Destroy crate
                                    Destroy(c.gameObject);
                                    Debug.Log("Something in the way");
                                }
                                else
                                {
                                    c.initCrate(leftFace);
                                    spawnedCrates.Enqueue(c);
                                    earthSound.Play();
                                }
                                break;
                            case PlayerDirection.Right:
                                if (Physics2D.Raycast(transform.position, transform.position - leftFace, 2.5f, crate))
                                {
                                    //Destroy crate
                                    Destroy(c.gameObject);
                                    Debug.Log("Something in the way");
                                }
                                else
                                {
                                    c.initCrate(rightFace);
                                    spawnedCrates.Enqueue(c);
                                    earthSound.Play();
                                }
                                break;
                            default:
                                Destroy(c.gameObject);
                                //Debug.Log("DEFAULT");
                                break;

                        }
                        //Deletes oldest crate to make room for the new one
                        if(spawnedCrates.Count > maxCrates)
                        {
                            Debug.Log("too many crates");
                            Crate dead = spawnedCrates.Dequeue();
                            dead.destroyObject();
                        }
                    break;
                    //Casting Force push
                    case 2:
                        //Debug.Log("FORCE BURST");
                        ForcePush f = Instantiate(poosh);
                        Vector3 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                        var d = Input.mousePosition - screenPoint;
                        d.Normalize();
                        f.initForcePush(gameObject.transform.position + d * 2.0f, d * 50f);
                        pushSound.Stop();
                        pushSound.Play();
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
                            spawnedVines.Enqueue(v);
                        }
                        if(spawnedVines.Count > maxVines)
                        {
                            Debug.Log("Too many vines");
                            spawnedVines.Dequeue().destroyObject();
                        }
                    break;
                    //Casting Timestop
                    case 4:
                        //Debug.Log("ZA WARUDOOOOOOO");
                        if(!sigilOut)
                        {
                            outStop = Instantiate(sigil);
                            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                            float e = 0.0f;
                            if (nearPlane.Raycast(r, out e))
                            {
                                Vector3 hitPoint = r.GetPoint(e);
                                outStop.initTimestop(new Vector3(hitPoint.x, hitPoint.y, transform.position.z));
                            }
                            timestopSound.Play();
                        }
                        else
                        {
                            outStop.releaseTimestop();
                        }
                        sigilOut = !sigilOut;
                        break;
                    //Default has nothing if no spell
                    default:
                        Debug.Log("YOU HAVE NO POWER HERE");
                        break;
                    }
                }
            }
            

        if(Input.GetKeyDown(KeyCode.Q))
        {
            inventory.prevItem();
            spellUI.cycleSpells();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.nextItem();
            spellUI.cycleSpells();
        }

    }

}
