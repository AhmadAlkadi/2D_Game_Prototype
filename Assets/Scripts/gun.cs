using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class gun : MonoBehaviour
{
    public enum GUN_TYPE {NORMAL, MACHINE, SPREAD, FLAME, LASER };
    public GUN_TYPE currentGunType = GUN_TYPE.MACHINE;

    public float attackCoolDownNormal = 0.5f;
    public float attackCoolDownMachine = 0.07f;
    public float attackCoolDownFlame = 0.35f;
    public float attackCoolDownLaser = 0.01f;
    public float fireRate = 10.0f;
    public float rapidFireRate = 1.05f;
    public int laserLength = 10;
    public float laserSpacing = 0.15f;
    [Range(0.0f, 2.0f)]
    public float spreadShotDirection = 1.0f;
    public bool allowRapidFire = false;

    public GameObject[] bulletObjects { get; private set; }
    public int numberOfBullets = 20;
    public GameObject playerDirection;

    [SerializeField] private GameObject bulletNormal, bulletMachine, bulletSpread, bulletFlame;
    [SerializeField] private GameObject turret_bullet;
    private List<GameObject> pBullets = new List<GameObject>();
    private List<TurretBullet> bullets = new List<TurretBullet>();
    private List<(int, float)> laserIndexList = new List<(int, float)>();

    private GameObject shootLocation;
    private float cooldownTimer = Mathf.Infinity;
    private float attackCoolDown = 1.0f;
    private Vector3 gunDirection;

    private GameObject parent;

    public void SetGun(GUN_TYPE gun_type)
    {
        currentGunType = gun_type;
    }

    public void SetRapidFire(bool allow)
    {
        allowRapidFire = allow;
    }

    public void SetDirection(Vector3 direction)
    {
        gunDirection = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = this.gameObject;
        while (parent.transform.parent != null)
        {
            parent = transform.parent.gameObject;
        }

        bulletObjects = new GameObject[] { bulletNormal, bulletMachine, bulletSpread, bulletFlame };
        const int target_location_child_index = 1; // relies on the target_location gameobject being the 4th child at the moment
        shootLocation = this.transform.GetChild(target_location_child_index).gameObject;

        for (int i=0; i<numberOfBullets; i++)
        {
            var newBullet = Instantiate(turret_bullet);
            newBullet.gameObject.layer = LayerMask.NameToLayer("Player");
            pBullets.Add(newBullet);

            var bulletChild = newBullet.transform.GetChild(0).gameObject.GetComponent<TurretBullet>();
            bulletChild.gameObject.layer = LayerMask.NameToLayer("Player");
            bulletChild.gameObject.GetComponent<CircleCollider2D>().excludeLayers |= (1 << LayerMask.NameToLayer("Player")) ;
            bullets.Add(bulletChild);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float yInput = Input.GetAxis("Vertical") != 0.0f ? Mathf.Sign(Input.GetAxis("Vertical")) : 0.0f;
        float xInput = Input.GetAxis("Horizontal") != 0.0f ? Mathf.Sign(Input.GetAxis("Horizontal")) : 0.0f;

        gunDirection = new Vector3(xInput, yInput, 0.0f);

        if (gunDirection == Vector3.zero)
        {
            gunDirection = shootLocation.transform.position - transform.position;
        }

        Vector3 normGunDirection = gunDirection.normalized;

        Vector3 shoot_direction = transform.position - shootLocation.transform.position;
        shoot_direction.Normalize();

        float playerScaleX = playerDirection.transform.localScale.x;
        float isFlippedY = playerScaleX >= 0.0f ? normGunDirection.y : -normGunDirection.y;

        float rotationRad = Mathf.Atan2(isFlippedY, Mathf.Abs(normGunDirection.x));
        float rotationDeg = Mathf.Rad2Deg * rotationRad;

        transform.eulerAngles = new Vector3(0.0f, 0.0f, rotationDeg);

        if (Input.GetKey(KeyCode.K) && (cooldownTimer > attackCoolDown))
        {
            float localFireRate = fireRate;

            if (allowRapidFire)
            {
                localFireRate *= rapidFireRate;
            }

            switch (currentGunType)
            {
                case GUN_TYPE.NORMAL:
                    attackCoolDown = attackCoolDownNormal;
                    ShootNormal(normGunDirection, localFireRate);
                    break;

                case GUN_TYPE.MACHINE:
                    attackCoolDown = attackCoolDownMachine;
                    ShootMachine(normGunDirection, localFireRate);
                    break;

                case GUN_TYPE.SPREAD:
                    attackCoolDown = attackCoolDownNormal;
                    ShootSpread(normGunDirection, localFireRate);
                    break;

                case GUN_TYPE.FLAME:
                    attackCoolDown = attackCoolDownFlame;
                    ShootFlame(normGunDirection, localFireRate);
                    break;

                case GUN_TYPE.LASER:
                    attackCoolDown = attackCoolDownLaser;
                    GenerateLaser(laserLength, laserSpacing);
                    ShootLaser(normGunDirection, localFireRate, laserIndexList, laserLength);
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
                bullets[index].SetPosition(shootLocation.transform.position.x, shootLocation.transform.position.y);
                bullets[index].SetOffsetPoint(new Vector3(0.0f, 0.0f));
                bullets[index].SetSpeed(bullet_speed);
                bullets[index].SetRotateSpeed(0.0f);
                bullets[index].SetRadius(0.0f);
                bullet_direction.Normalize();

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
                bullets[index].SetPosition(shootLocation.transform.position.x, shootLocation.transform.position.y);
                bullets[index].SetOffsetPoint(new Vector3(0.0f, 0.0f));
                bullets[index].SetSpeed(bullet_speed);
                bullets[index].SetRotateSpeed(0.0f);
                bullets[index].SetRadius(0.0f);
                bullet_direction.Normalize();

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
            List<(int, float)> indexList = new List<(int, float)>();
            float spread_shot_direction = -spreadShotDirection;
            float spread_shot_delta = MathF.Abs(spread_shot_direction * 2.0f) / (numberOfBulletsShot - 1.0f);

            for (int i = 0; i < numberOfBulletsShot; i++)
            {
                int index = FindInActiveBullet(indexList);

                if (index >= 0)
                {
                    indexList.Add((index, spread_shot_direction));
                    spread_shot_direction += spread_shot_delta;
                }
            }

            if (indexList.Count == numberOfBulletsShot)
            {
                foreach ((int index, float spread_shot) in indexList)
                {
                    bullets[index].SetPosition(shootLocation.transform.position.x, shootLocation.transform.position.y);
                    bullets[index].SetOffsetPoint(new Vector3(0.0f, 0.0f));
                    bullets[index].SetSpeed(bullet_speed);
                    bullets[index].SetRotateSpeed(0.0f);
                    bullets[index].SetRadius(0.0f);
                    bullet_direction.Normalize();

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
                float local_radius = 1.0f;
                float local_offset_x = 1.5f;
                float local_offset_y = 1.5f;

                float x_direction = (shootLocation.transform.position - parent.transform.position).x;
                float y_direction = (shootLocation.transform.position - parent.transform.position).y;
                if (x_direction < 0.0f)
                {
                    local_offset_x = -local_offset_x;
                }

                if (y_direction < 0.0f)
                {
                    local_offset_y = -local_offset_y;
                }

                bullets[index].SetPosition(parent.transform.position.x, parent.transform.position.y);
                bullets[index].SetOffsetPoint(new Vector3(local_offset_x, 0.0f));
                bullets[index].SetSpeed(bullet_speed);
                bullets[index].SetRotateSpeed(10.0f);
                bullets[index].SetRadius(local_radius);
                bullet_direction.Normalize();

                if (bullet_direction.x == 0)
                {
                    bullets[index].SetOffsetPoint(new Vector3(0.0f, local_offset_y));
                    bullets[index].SetDirection(0.0f, Mathf.Sign(bullet_direction.y));
                }
                else if (bullet_direction.y == 0)
                {
                    bullets[index].SetDirection(Mathf.Sign(bullet_direction.x), 0.0f);
                }
                else
                {
                    bullets[index].SetOffsetPoint(new Vector3(local_offset_x, local_offset_y));
                    bullets[index].SetDirection(Mathf.Sign(bullet_direction.x), Mathf.Sign(bullet_direction.y));
                }
            }
        }
    }

    private void GenerateLaser(int numberOfBulletsShot, float laserSpacing)
    {
        // deactivate the last run of bullets before collecting the next batch
        if (laserIndexList.Count > 0)
        {
            foreach (var (index, spread) in laserIndexList)
            {
                bullets[index].Deactivate();
            }
        }

        // gather the number of bullets to generate laser
        laserIndexList.Clear();
        float spread_shot_direction = 0.0f;
        for (int i = 0; i < numberOfBulletsShot; i++)
        {
            int index = FindInActiveBullet(laserIndexList);

            if (index >= 0)
            {
                laserIndexList.Add((index, spread_shot_direction));
                spread_shot_direction += laserSpacing;
            }
        }
    }

    private void ShootLaser(Vector3 bullet_direction, float bullet_speed, List<(int, float)> indexList, int limit)
    {
        int numberOfBulletsShot = limit;
        bool shoot_target = true;
        if (shoot_target)
        {
            if (indexList.Count == numberOfBulletsShot)
            {
                foreach ((int index, float spread_shot) in indexList)
                {
                    bullets[index].SetPosition(shootLocation.transform.position.x, shootLocation.transform.position.y);
                    bullets[index].SetSpeed(bullet_speed);
                    bullets[index].SetRotateSpeed(0.0f);
                    bullets[index].SetRadius(0.0f);
                    bullet_direction.Normalize();

                    if (bullet_direction.x == 0)
                    {
                        bullets[index].SetOffsetPoint(new Vector3(0.0f, spread_shot));
                        bullets[index].SetDirection(0.0f, Mathf.Sign(bullet_direction.y));
                    }
                    else if (bullet_direction.y == 0)
                    {
                        bullets[index].SetOffsetPoint(new Vector3(spread_shot, 0.0f));
                        bullets[index].SetDirection(Mathf.Sign(bullet_direction.x), bullet_direction.y);
                    }
                    else
                    {
                        bullets[index].SetOffsetPoint(new Vector3(spread_shot, spread_shot));
                        bullets[index].SetDirection(Mathf.Sign(bullet_direction.x), Mathf.Sign(bullet_direction.y));
                    }
                }
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
