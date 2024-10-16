using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public enum GUN_TYPE { NORMAL, MACHINE, SPREAD, FLAME, LASER, RAPID };
    public GUN_TYPE itemPickUpType = GUN_TYPE.MACHINE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (collision.gameObject.GetComponentInChildren<gun>() != null)
            {
                gun currentGun = collision.gameObject.GetComponentInChildren<gun>();
                switch (itemPickUpType)
                {
                    case GUN_TYPE.NORMAL:
                        currentGun.SetGun(gun.GUN_TYPE.NORMAL);
                        break;
                    case GUN_TYPE.MACHINE:
                        currentGun.SetGun(gun.GUN_TYPE.MACHINE);
                        break;

                    case GUN_TYPE.SPREAD:
                        currentGun.SetGun(gun.GUN_TYPE.SPREAD);
                        break;

                    case GUN_TYPE.FLAME:
                        currentGun.SetGun(gun.GUN_TYPE.FLAME);
                        break;

                    case GUN_TYPE.LASER:
                        currentGun.SetGun(gun.GUN_TYPE.LASER);
                        break;

                    case GUN_TYPE.RAPID:
                        currentGun.SetRapidFire(true);
                        break;
                }
            }
        }
    }


}
