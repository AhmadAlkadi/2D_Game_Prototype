/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: this sets the items type and they vanish when picked up
*
*References:
*https://docs.unity3d.com/ScriptReference/index.html
*
****************************************************************/
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
                        gameObject.SetActive(false);
                        break;
                    case GUN_TYPE.MACHINE:
                        currentGun.SetGun(gun.GUN_TYPE.MACHINE);
                        gameObject.SetActive(false);
                        break;

                    case GUN_TYPE.SPREAD:
                        currentGun.SetGun(gun.GUN_TYPE.SPREAD);
                        gameObject.SetActive(false);
                        break;

                    case GUN_TYPE.FLAME:
                        currentGun.SetGun(gun.GUN_TYPE.FLAME);
                        gameObject.SetActive(false);
                        break;

                    case GUN_TYPE.LASER:
                        currentGun.SetGun(gun.GUN_TYPE.LASER);
                        gameObject.SetActive(false);
                        break;

                    case GUN_TYPE.RAPID:
                        currentGun.SetRapidFire(true);
                        gameObject.SetActive(false);
                        break;
                }
            }
        }
    }


}
