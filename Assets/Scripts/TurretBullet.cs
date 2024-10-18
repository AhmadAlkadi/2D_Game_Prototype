/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: The bullet of the turrent
*
*References:
*https://docs.unity3d.com/ScriptReference/index.html
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class TurretBullet : MonoBehaviour
{
    private float speed;
    private float rotationSpeed;
    private Vector3 offsetPoint;
    private Vector3 startPoint;
    private float directionX;
    private float directionY;
    private bool hit;
    private float rotation;
    private float lastRadius;
    private float radius;
    private CircleCollider2D circleCollider;
    private GameObject parentObject;
    private GameObject player;

    public void setPlayer(ref GameObject newPlayer)
    {
        player = newPlayer;
    }

    private void OnBecameInvisible()
    {
        Deactivate();
    }

    private void Start()
    {
    }

    private void Awake()
    {
        offsetPoint = Vector3.zero;
        rotation = 0.0f;
        radius = 1.0f;
        rotationSpeed = 10.0f;

        parentObject = (gameObject.transform.parent != null) ? gameObject.transform.parent.gameObject : null;
        while (parentObject.transform.parent != null)
        {
            parentObject = parentObject.transform.parent.gameObject;
        }

        circleCollider = GetComponent<CircleCollider2D>();

        parentObject.SetActive(false);

        this.transform.position = new Vector3(parentObject.transform.position.x + radius, parentObject.transform.position.y);
        lastRadius = radius;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!hit)
        {
            parentObject.transform.Rotate(0.0f, 0.0f, -rotation);

            if ((lastRadius != radius) && (radius != 0.0f))
            {
                Vector3 p_pos = parentObject.transform.position;
                Vector3 c_pos = this.transform.position;

                Vector3 n_pos = c_pos - p_pos;
                n_pos.Normalize();
                n_pos *= radius;
                n_pos += p_pos;

                this.transform.position -= this.transform.position;
                this.transform.position = n_pos;
                lastRadius = radius;
            }

            rotation -= rotationSpeed;

            float movementSpeedX = speed * Time.deltaTime * directionX;
            float movementSpeedY = speed * Time.deltaTime * directionY;

            parentObject.transform.Translate(movementSpeedX, movementSpeedY, 0.0f);
            parentObject.transform.Rotate(0.0f, 0.0f, rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != null)
        {
            if (collision.gameObject.layer == 6)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.setPlayerHit(true);
                hit = true;
            }
        }

        if (gameObject.layer == 6)
        {
            if (collision.gameObject.layer == 8)
            {
                EntityMovement entityMovement = collision.gameObject.GetComponent<EntityMovement>();
                entityMovement.setFootSoldierHit(true);
                hit = true;
            }
        }

        if (gameObject.layer == 6)
        {
            if (collision.gameObject.layer == 10)
            {
                Turret_Targeting turret_Targeting = collision.gameObject.GetComponent<Turret_Targeting>();
                if (turret_Targeting != null)
                {
                    turret_Targeting.setTurrentHit(true);
                    hit = true;
                }
            }
        }

        hit = true;
        Deactivate();
        circleCollider.enabled = false;
    }

    public void SetSpeed(float m_speed)
    {
        speed = m_speed;
    }

    public void SetRotateSpeed(float m_speed)
    {
        rotationSpeed = m_speed;
    }

    public void SetOffsetPoint(Vector3 offsetPoint)
    {
        this.offsetPoint = offsetPoint;
    }

    public void SetPosition(float x, float y)
    {
        startPoint = new Vector3(x, y);
    }

    public void SetRadius(float r)
    {
        radius = r;
    }

    public void SetDirection(float x, float y)
    {
        directionX = x;
        directionY = y;

        parentObject.transform.position = new Vector3(startPoint.x + offsetPoint.x, startPoint.y + offsetPoint.y);
        this.transform.position = new Vector3(parentObject.transform.position.x + radius, parentObject.transform.position.y);
        
        parentObject.SetActive(true);
        hit = false;
        circleCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != x)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Deactivate()
    {
        parentObject.SetActive(false);
    }
}
