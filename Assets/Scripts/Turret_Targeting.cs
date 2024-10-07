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
    [SerializeField] private TurretBullet turret_bullet;
    public GameObject target_position;
    public float detection_distance = 5.0f;

    private GameObject shootLocation;
    private List<TurretBullet> bullets = new List<TurretBullet>();
    private TurretBullet lastBullet = null;

    // Start is called before the first frame update
    void Start()
    {
        shootLocation = this.transform.GetChild(4).gameObject;

        for (int i = 0; i < 10; i++)
        {
            bullets.Add(Instantiate(turret_bullet));
        }
    }

    // Update is called once per frame
    void Update()
    {
        var this_obj_position = GetComponent<Transform>();
        var direction = this_obj_position.transform.position - target_position.transform.position;
        var distance = direction.magnitude;
        direction.Normalize();

        float turret_rotation_rad = Mathf.Atan2(direction.y, direction.x);
        float turret_rotation_deg = Mathf.Rad2Deg * turret_rotation_rad;

        //print(turret_rotation_deg);

        int fixed_positions = 45;
        int fixed_rot = (int)((turret_rotation_deg / fixed_positions) + 0.5);

        turret_rotation_deg = Mathf.Abs((float)fixed_rot * 45.0f);

        //print(direction.y);
        //print(turret_rotation_deg);
        print(distance);

        if (direction.y < 0)
        {
            turret_rotation_deg *= -1.0f;
        }

        // TODO: bullets are shooting but sometimes up with the math
        if (distance < detection_distance)
        {
            bool shoot = true;
            Vector3 bullet_direction = shootLocation.transform.position - this_obj_position.transform.position;
            float bullet_distance = bullet_direction.magnitude;

            if (lastBullet != null)
            {
                bullet_direction = this_obj_position.transform.position - lastBullet.transform.position;
                bullet_distance = bullet_direction.magnitude;

                if (Mathf.Abs(bullet_distance) < 5.0f)
                {
                    shoot = false;
                }
            }

            if (shoot)
            {
                int index = FindBullet();

                bullets[index].transform.position = shootLocation.transform.position;
                bullets[index].SetSpeed(1.0f);
                bullets[index].SetDirection(bullet_direction.x, bullet_direction.y);

                lastBullet = bullets[index];
            }
        }


        //print(turret_rotation_deg);


        this_obj_position.transform.eulerAngles = new Vector3(0.0f, 0.0f, turret_rotation_deg);
    }

    
    private int FindBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }
        
        return 0;
    }
    
}
