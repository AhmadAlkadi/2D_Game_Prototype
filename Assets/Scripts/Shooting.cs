/***************************************************************
*file: DoubleIt.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: In here this is where the bullet will come out from and the attack delay of the bullet is also set here
*
**References:
*Creator name: Pandemonium, link of the video that was used: https://www.youtube.com/watch?v=PUpC44Q64zY&list=PLgOEwFbvGm5o8hayFB6skAfa8Z-mw4dPV&index=4
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] bullets;
    public float pivot = 0.69f;
    public PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    public bool RTPivot = false;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        firePoint.localPosition = new Vector3(pivot, firePoint.localPosition.y, firePoint.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (RTPivot)
        {
            firePoint.localPosition = new Vector3(pivot, firePoint.localPosition.y, firePoint.localPosition.z);
        }
        if (Input.GetKey(KeyCode.V) && cooldownTimer > attackCoolDown && playerMovement.canAttak())
        {
            Attack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        cooldownTimer = 0;
        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");

        bullets[FindBullet()].transform.position = firePoint.position;

        firePoint.localPosition = new Vector3(0, 0, firePoint.localPosition.z);

        if (Mathf.Abs(xInput) > 0) 
        {
            firePoint.localPosition = new Vector3(pivot, firePoint.localPosition.y, firePoint.localPosition.z);
        }

        if (yInput > 0)
        {
            firePoint.localPosition = new Vector3(firePoint.localPosition.x, pivot, firePoint.localPosition.z);
        }

        if (yInput < 0)
        {
            firePoint.localPosition = new Vector3(firePoint.localPosition.x, -pivot, firePoint.localPosition.z);
        }

        if(firePoint.localPosition.x == 0 && firePoint.localPosition.y == 0)
        {
            firePoint.localPosition = new Vector3(pivot, firePoint.localPosition.y, firePoint.localPosition.z);
        }
        
        if(xInput == 0)
        {
            float xDirection = Mathf.Sign(firePoint.localPosition.x);
            if(firePoint.localPosition.x == 0)
            {
                xDirection = 0;
            }
            if (Mathf.Sign(transform.localScale.x) > 0)
            {
                bullets[FindBullet()].GetComponent<Projectile>().SetDirection(xDirection, firePoint.localPosition.y);
            }
            else
            {
                bullets[FindBullet()].GetComponent<Projectile>().SetDirection(-xDirection, firePoint.localPosition.y);
            }
        }
        else
        {
            bullets[FindBullet()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x), firePoint.localPosition.y);
        }
    }

    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
            return i;
        }
        return 0;
    }

}
