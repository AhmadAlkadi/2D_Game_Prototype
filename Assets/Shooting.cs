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
    public PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V) && cooldownTimer > attackCoolDown && playerMovement.canAttak())
        {
            Attack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        cooldownTimer = 0;

        bullets[FindBullet()].transform.position = firePoint.position;
        bullets[FindBullet()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
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
