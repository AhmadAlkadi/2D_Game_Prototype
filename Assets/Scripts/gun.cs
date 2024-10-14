using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public enum GUN_TYPE {NORMAL, MACHINE, SPREAD, FLAME, LASER };
    public GUN_TYPE currentGunType = GUN_TYPE.MACHINE;

    public float attackCoolDownNormal = 0.5f;
    public float attackCoolDownMachine = 0.07f;
    public float attackCoolDownFlame = 0.35f;
    public float rapidFireRate = 1.05f;
    public bool allowRapidFire = false;

    public GameObject[] bulletObjects { get; private set; }
    public int numberOfBullets = 20;

    [SerializeField] private GameObject bulletNormal, bulletMachine, bulletSpread, bulletFlame;
    [SerializeField] private TurretBullet turret_bullet;
    private List<TurretBullet> bullets = new List<TurretBullet>();

    private GameObject shootLocation;
    private float cooldownTimer = Mathf.Infinity;
    private float attackCoolDown = 1.0f;

    void SetGun(GUN_TYPE gun_type)
    {
        currentGunType = gun_type;
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletObjects = new GameObject[] { bulletNormal, bulletMachine, bulletSpread, bulletFlame };
        const int target_location_child_index = 1; // relies on the target_location gameobject being the 4th child at the moment
        shootLocation = this.transform.GetChild(target_location_child_index).gameObject;

        for (int i=0; i<numberOfBullets; i++)
        {
            var newBullet = Instantiate(turret_bullet);
            bullets.Add(newBullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K) && (cooldownTimer > attackCoolDown))
        {
            float fireRate = 10.0f;

            if (allowRapidFire)
            {
                fireRate *= rapidFireRate;
            }

            switch (currentGunType)
            {
                case GUN_TYPE.NORMAL:
                    attackCoolDown = attackCoolDownNormal;
                    ShootNormal(Vector3.right, fireRate);
                    break;

                case GUN_TYPE.MACHINE:
                    attackCoolDown = attackCoolDownMachine;
                    ShootMachine(Vector3.right, fireRate);
                    break;

                case GUN_TYPE.SPREAD:
                    attackCoolDown = attackCoolDownNormal;
                    ShootSpread(Vector3.right, fireRate);
                    break;

                case GUN_TYPE.FLAME:
                    attackCoolDown = attackCoolDownFlame;
                    ShootFlame(Vector3.right, fireRate);
                    break;

                case GUN_TYPE.LASER:
                    attackCoolDown = attackCoolDownNormal;
                    ShootLaser(Vector3.right, fireRate);
                    break;
            }

            cooldownTimer = 0.0f;
        }

        cooldownTimer += Time.deltaTime;
    }

    private void ShootNormal(Vector3 bullet_direction, float bullet_speed)
    {
        bool shoot_target = true;
        if (shoot_target)
        {
            int index = FindInActiveBullet();

            if (index >= 0)
            {
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
            }
        }
    }

    private void ShootMachine(Vector3 bullet_direction, float bullet_speed)
    {
        bool shoot_target = true;
        if (shoot_target)
        {
            int index = FindInActiveBullet();

            if (index >= 0)
            {
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
            }
        }
    }

    private void ShootSpread(Vector3 bullet_direction, float bullet_speed)
    {
        const int numberOfBulletsShot = 5;
        bool shoot_target = true;
        if (shoot_target)
        {
            List<(int,float)> indexList = new List<(int, float)>();
            float spread_shot_direction = -1.0f;
            for (int i=0; i < numberOfBulletsShot; i++)
            {
                int index = FindInActiveBullet(indexList);

                if (index >= 0)
                {
                    indexList.Add((index, spread_shot_direction));
                    spread_shot_direction += 0.5f;
                }
            }

            if (indexList.Count == numberOfBulletsShot)
            {
                foreach ((int index, float spread_shot) in indexList)
                {
                    bullets[index].transform.position = shootLocation.transform.position;
                    bullet_direction.Normalize();
                    bullets[index].SetSpeed(bullet_speed);

                    if (bullet_direction.x == 0)
                    {
                        bullets[index].SetDirection(spread_shot, Mathf.Sign(bullet_direction.y));
                    }
                    else if (bullet_direction.y == 0)
                    {
                        bullets[index].SetDirection(Mathf.Sign(bullet_direction.x), spread_shot);
                    }
                    else
                    {
                        bullets[index].SetDirection(Mathf.Sign(bullet_direction.x), Mathf.Sign(bullet_direction.y));
                    }
                }
            }
        }
    }

    private void ShootFlame(Vector3 bullet_direction, float bullet_speed)
    {
        bool shoot_target = true;
        if (shoot_target)
        {
            int index = FindInActiveBullet();

            if (index >= 0)
            {
                bullets[index].SetPivotPoint(new Vector3(5.0f, 0.0f, 0.0f));
                bullets[index].SetRotateSpeed(100.0f);
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
            }
        }
    }

    private void ShootLaser(Vector3 bullet_direction, float bullet_speed)
    {

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

    private int FindInActiveBullet(in List<(int, float)> list)
    {
        int bullet_index = -1;

        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].gameObject.activeInHierarchy && !list.Exists(x => x.Item1 == i))
            {
                bullet_index = i;
                break;
            }
        }

        return bullet_index;
    }
}
