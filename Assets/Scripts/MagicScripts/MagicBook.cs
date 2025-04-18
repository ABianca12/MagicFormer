using UnityEngine;

public class MagicBook : Collectable
{
    //Member variables
    [SerializeField] private int magicIndex = 0;

    //Init functions
    public void initMagicBook()
    {
        magicIndex = 0;
        initCollectable();
    }
    public void initMagicBook(Vector3 start)
    {
        magicIndex = 0;
        initCollectable(start);
    }
    public void initMagicBook(Vector3 start, int index)
    {
        magicIndex = index;
        initCollectable(start);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        //if Player collides with the object
        if (c.gameObject.tag == "Player")
        {
            //Collect Item
            c.gameObject.GetComponent<Inventory>().addSpell(magicIndex);
            Destroy(gameObject);
        }
    }
}
