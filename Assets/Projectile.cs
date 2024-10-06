/***************************************************************
*file: DoubleIt.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: This programs is for the bullet, setting the speed of the bullet and when it attacks an object
*
**References:
*Creator name: Pandemonium, link of the video that was used: https://www.youtube.com/watch?v=PUpC44Q64zY&list=PLgOEwFbvGm5o8hayFB6skAfa8Z-mw4dPV&index=4
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    Renderer m_Renderer;

    private void OnBecameInvisible()
    {
        Deactivate();
    }

    private BoxCollider2D boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (hit) { return; }
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit  = true;
        boxCollider.enabled = false;
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled=true;

        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direction) { localScaleX = -localScaleX; }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.x) ;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
