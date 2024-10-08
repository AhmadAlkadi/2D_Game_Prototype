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
    public float detection_distance = 10.0f;
    public float bullet_distance_to_shoot = 10.0f;
    public float bullet_speed = 4.0f;
    public int number_of_bullets = 10;
    public float turret_angle_movement = 45.0f;

    [SerializeField] private TurretBullet turret_bullet;
    [SerializeField] private List<int> ignoreAngles;
    private GameObject shootLocation;
    private List<TurretBullet> bullets = new List<TurretBullet>();
    private int last_fixed_rot = 0;
    private TurretBullet lastBullet = null;

    // Start is called before the first frame update
    void Start()
    {
        const int target_location_child_index = 4; // relies on the target_location gameobject being the 4th child at the moment
        shootLocation = this.transform.GetChild(target_location_child_index).gameObject;

        for (int i = 0; i < number_of_bullets; i++)
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

        int fixed_positions = ((int)turret_angle_movement == 0) ? 1 : ((int)turret_angle_movement);   

        int fixed_rot = (int)((turret_rotation_deg / (float)fixed_positions) + 0.5f);
        bool allow_rotation = CheckAnglesToIgnore(ref fixed_rot);

        turret_rotation_deg = Mathf.Abs((float)fixed_rot * turret_angle_movement);
        turret_rotation_deg = (direction.y < 0) ? turret_rotation_deg *= -1.0f : turret_rotation_deg;

        if (distance < detection_distance)
        {
            Vector3 bullet_direction = shootLocation.transform.position - this_obj_position.transform.position;
            bool shoot_target = CheckShootTarget();
            ShootTarget(bullet_direction, shoot_target);
        }

        if (allow_rotation)
        {
            last_fixed_rot = fixed_rot;
        }
        
        this_obj_position.transform.eulerAngles = new Vector3(0.0f, 0.0f, turret_rotation_deg);
    }

    private bool CheckAnglesToIgnore(ref int fixed_rot)
    {
        bool allow_rotation = true;
        foreach (var angle in ignoreAngles)
        {
            if (angle == fixed_rot)
            {
                allow_rotation = false;
                fixed_rot = last_fixed_rot;
                break;
            }
        }

        return allow_rotation;
    }

    private bool CheckShootTarget()
    {
        bool shoot_target = true;
        if (lastBullet != null)
        {
            Vector3 last_bullet_direction = this.transform.position - lastBullet.transform.position;
            float last_bullet_distance = last_bullet_direction.magnitude;

            if (Mathf.Abs(last_bullet_distance) < bullet_distance_to_shoot)
            {
                shoot_target = false;
            }
        }

        return shoot_target;
    }

    private void ShootTarget(Vector3 bullet_direction, bool shoot_target)
    {
        if (shoot_target)
        {
            int index = FindBullet();

            bullets[index].transform.position = shootLocation.transform.position;
            bullet_direction.Normalize();
            bullets[index].SetSpeed(bullet_speed);

            if (bullet_direction.x == 0)
            {
                bullets[index].SetDirection(0.0f, Mathf.Sign(bullet_direction.y));
            }
            else if (bullet_direction.y == 0)
            {
                bullets[index].SetDirection(Mathf.Sign(bullet_direction.x), 0.0f);
            }
            else
            {
                bullets[index].SetDirection(Mathf.Sign(bullet_direction.x), Mathf.Sign(bullet_direction.y));
            }

            lastBullet = bullets[index];
        }
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
