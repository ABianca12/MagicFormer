using UnityEngine;

public class ForcePush : Projectile
{
    
    //init functions
    public void initForcePush()
    {
        initProjectile(BaseType.MYSTIC, true, 1.0f);
    }

    //Update is called once per frame
    void Update()
    {
        base.updateEntity(Time.deltaTime);
    }
}
