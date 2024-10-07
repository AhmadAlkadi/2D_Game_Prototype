/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: The turrent will target the player and shoot
*
*References:
*N/A
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Targeting : MonoBehaviour
{
    public GameObject target_position;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var this_obj_position = GetComponent<Transform>();
        var direction = this_obj_position.transform.position - target_position.transform.position;
        direction.Normalize();

        float turret_rotation_rad = Mathf.Atan2(direction.y, direction.x);
        float turret_rotation_deg = Mathf.Rad2Deg * turret_rotation_rad;

        

        if (direction.x > 0.0)
        {
            if (turret_rotation_deg >= 45.0)
            {
                turret_rotation_deg = 45.0f;
            }
            else if (turret_rotation_deg <= -45.0)
            {
                turret_rotation_deg = -45.0f;
            }
            else
            {
                turret_rotation_deg = 0.0f;
            }
        }
        else
        {
            if (turret_rotation_deg <= -135 || turret_rotation_deg >= 135)
            {
                turret_rotation_deg = 180.0f;
            }
            else if (turret_rotation_deg >= -135.0 && turret_rotation_deg <= -90.0)
            {
                turret_rotation_deg = -135.0f;
            }
            else // if (turret_rotation_deg <= 135)
            {
                turret_rotation_deg = 135.0f;
            }

        }

        this_obj_position.transform.eulerAngles = new Vector3(0.0f, 0.0f, turret_rotation_deg);
    }
}
