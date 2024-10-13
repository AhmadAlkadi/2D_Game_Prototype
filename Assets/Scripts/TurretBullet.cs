/***************************************************************
*file: TurretBullet.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 � Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: N/A
*
*References:
*N/A
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private float speed;
    private float directionX;
    private float directionY;
    private bool hit;
    private CircleCollider2D circleCollider;

    private void OnBecameInvisible()
    {
        Deactivate();
    }

    private void Start()
    {
        gameObject.SetActive(false);
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!hit)
        {
            float movementSpeedX = speed * Time.deltaTime * directionX;
            float movementSpeedY = speed * Time.deltaTime * directionY;
            transform.Translate(movementSpeedX, movementSpeedY, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        Deactivate();
        circleCollider.enabled = false;
    }

    public void SetSpeed(float m_speed)
    {
        speed = m_speed;
    }

    public void SetDirection(float _directionX, float _directionY)
    {
        directionX = _directionX;
        directionY = _directionY;
        gameObject.SetActive(true);
        hit = false;
        circleCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _directionX)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
