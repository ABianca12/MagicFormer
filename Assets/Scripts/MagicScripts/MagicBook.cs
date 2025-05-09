using UnityEngine;

public class MagicBook : Collectable
{
    //Member variables
    [SerializeField] private int magicIndex = 0;
    private SpellUI spellUI;

    //Init functions
    public void initMagicBook()
    {
        magicIndex = 0;
        initCollectable();
        spellUI = GameObject.FindWithTag("SpellUI").GetComponent<SpellUI>();
    }
    public void initMagicBook(Vector3 start)
    {
        magicIndex = 0;
        initCollectable(start);
        spellUI = GameObject.FindWithTag("SpellUI").GetComponent<SpellUI>();
    }
    public void initMagicBook(Vector3 start, int index)
    {
        magicIndex = index;
        initCollectable(start);
        spellUI = GameObject.FindWithTag("SpellUI").GetComponent<SpellUI>();
    }

    private void Start()
    {
        initMagicBook(transform.position, magicIndex);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        //if Player collides with the object
        if (c.gameObject.tag == "Player")
        {
            //Collect Item
            c.gameObject.GetComponent<Inventory>().addSpell(magicIndex);
            spellUI.cycleSpells();
            Destroy(gameObject);
        }
    }
}
