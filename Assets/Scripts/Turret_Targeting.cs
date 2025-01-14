/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 � Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: The turrent will target the player and shoot
*
*References:
*https://docs.unity3d.com/ScriptReference/index.html
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Targeting : MonoBehaviour
{
    public GameObject target_position;
    public float detection_distance = 10.0f;
    public float bullet_tts_seconds = 0.5f;
    public float bullet_speed = 4.0f;
    public int number_of_bullets = 10;
    public float turret_angle_movement = 45.0f;
    public GameObject player;
    public bool enable = true;
    public bool ForceRun = false;
    public int Health = 8;

    [SerializeField] private GameObject turret_bullet;
    private List<GameObject> pbullets = new List<GameObject>();
    private List<TurretBullet> bullets = new List<TurretBullet>();
    [SerializeField] private List<int> ignoreAngles;
    private GameObject shootLocation;
    private int last_fixed_rot = 0;
    private float timeToShootSeconds = 0.0f;
    private bool hit;

    public void setTurrentHit(bool hit)
    {
        if (enable)
        {
            this.hit = hit;
            Health -= 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        const int target_location_child_index = 4; // relies on the target_location gameobject being the 4th child at the moment
        shootLocation = this.transform.GetChild(target_location_child_index).gameObject;


        for (int i = 0; i < number_of_bullets; i++)
        {
            var pBullet = Instantiate(turret_bullet);
            pbullets.Add(pBullet);

            var newBullet = pBullet.GetComponentInChildren<TurretBullet>();
            bullets.Add(newBullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((Health <= 0) && (ForceRun == false))
        {
            gameObject.SetActive(false);

        }
        else
        { 
            timeToShootSeconds += Time.deltaTime;
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
                if (enable)
                {
                    Vector3 bullet_direction = shootLocation.transform.position - this_obj_position.transform.position;
                    bool shoot_target = CheckShootTarget();
                    ShootTarget(bullet_direction, shoot_target);
                }
            }

            if (allow_rotation)
            {
                last_fixed_rot = fixed_rot;
            }
        
            this_obj_position.transform.eulerAngles = new Vector3(0.0f, 0.0f, turret_rotation_deg);
        }
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
        bool shoot_target = false;

        if (timeToShootSeconds > bullet_tts_seconds)
        {
            timeToShootSeconds = 0.0f;
            shoot_target = true;
        }

        return shoot_target;
    }

    private void ShootTarget(Vector3 bullet_direction, bool shoot_target)
    {
        if (shoot_target)
        {
            int index = FindInActiveBullet();

            if (index >= 0)
            {
                bullets[index].setPlayer(ref player);
                bullets[index].SetPosition(shootLocation.transform.position.x, shootLocation.transform.position.y);
                bullets[index].SetOffsetPoint(new Vector3(0.0f, 0.0f));
                bullets[index].SetSpeed(bullet_speed);
                bullets[index].SetRotateSpeed(0.0f);
                bullets[index].SetRadius(0.0f);
                bullet_direction.Normalize();

                if (Mathf.Abs(bullet_direction.x) <= 0.001f)
                {
                    bullet_direction.x = 0.0f;
                }
                else
                {
                    bullet_direction.x = Mathf.Sign(bullet_direction.x);
                }

                if (Mathf.Abs(bullet_direction.y) <= 0.001f)
                {
                    bullet_direction.y = 0.0f;
                }
                else
                {
                    bullet_direction.y = Mathf.Sign(bullet_direction.y);
                }

                bullets[index].SetDirection(bullet_direction.x, bullet_direction.y);
            }
        }
    }
    
    private int FindInActiveBullet()
    {
        int bullet_index = -1;

        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].gameObject.activeInHierarchy)
            {
                bullet_index = i;
                break;
            }
        }
        
        return bullet_index;
    }

}
