using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    public Turret_Targeting turretLeft;
    public Turret_Targeting turretRight;
    public GameObject turretCore;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        turretLeft.enable = true;
        turretRight.enable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        turretLeft.enable = false;
        turretRight.enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
