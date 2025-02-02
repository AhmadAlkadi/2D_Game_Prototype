/***************************************************************
*file: Turret_Targeting.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 � Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: Enemy movement behaviour
*
*References:
*https://docs.unity3d.com/ScriptReference/index.html
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EntityMovement : MonoBehaviour
{
    private GameObject cameraLeftColid;
    private GameObject cameraRightColid;
    private GameObject edgeRightColid;
    private GameObject edgeLeftColid;
    private GameObject edgeBottomColid;
    private float jumpHight;
    private GameObject player;
    private float speed;
    private int edgeWalkBack = 0;
    private bool rightMove;
    private new Rigidbody2D rigidbody;
    private bool hit = false;
    private bool isLeftEdgeTouched = false;
    private bool isRightEdgeTouched = false;


    private Vector3 startPosition;

    public void setPlayer(ref GameObject player)
    {
        this.player = player;
    }

    public void setFootSoldierHit(bool hit)
    {
        this.hit = hit;
    }

    public void setEdgeWalkBack(int edgeWalkBack)
    {
        this.edgeWalkBack = edgeWalkBack;
    }

    public void setJumpHight(ref float newJumpHight)
    {
        jumpHight = newJumpHight;
    }

    public void setSpeed(ref float newSpeed)
    {
        speed = newSpeed;
    }

    public void setEdgeRightColid(ref GameObject newEdgeRightColid)
    {
        edgeRightColid = newEdgeRightColid;
    }

    public void setEdgeLeftColid(ref GameObject newEdgeLeftColid)
    {
        edgeLeftColid = newEdgeLeftColid;
    }

    public void setEdgeBottomColid(ref GameObject newEdgeBottomColid)
    {
        edgeBottomColid = newEdgeBottomColid;
    }

    public void setCameraRightColid(ref GameObject extraCameraRightColid)
    {
        cameraRightColid = extraCameraRightColid;
    }

    public void setCameraLeftColid(ref GameObject extraCameraLeftColid)
    {
        cameraLeftColid = extraCameraLeftColid;
    }

    public void setStartPosition(Vector3 startPos)
    {
        startPosition = startPos;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "LeftEdgeColider")
        {
            edgeLeftColid = collision.gameObject;
            isLeftEdgeTouched = true;
        }
        else if (collision.name == "RightEdgeColider")
        {
            edgeRightColid = collision.gameObject;
            isRightEdgeTouched = true;
        }
    }

    void Start()
    {
        gameObject.transform.position = startPosition;
    }

    void Update()
    {
        if (hit)
        {
            gameObject.SetActive(false);
            edgeWalkBack = 0;
            gameObject.transform.position = startPosition;
            hit = false;
        }
        else
        {
            if (gameObject.GetComponent<BoxCollider2D>().IsTouching(edgeBottomColid.GetComponent<BoxCollider2D>()))
            {
                hit = true;
            }

            if ((player != null) && (gameObject.GetComponent<BoxCollider2D>().IsTouching(player.GetComponent<BoxCollider2D>())))
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.setPlayerHit(true);
            }

            if (cameraLeftColid != null)
            {
                if (gameObject.GetComponent<BoxCollider2D>().IsTouching(edgeLeftColid.GetComponent<BoxCollider2D>()) || isLeftEdgeTouched)
                {
                    isLeftEdgeTouched = false;

                    if (edgeWalkBack == 0)
                    {
                        rightMove = true;
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        edgeWalkBack = 1;
                    }
                    if (edgeWalkBack == 1)
                    {
                        Rigidbody2D EnemyRid = gameObject.GetComponent<Rigidbody2D>();
                        EnemyRid.AddForce(Vector3.up * jumpHight);
                    }
                }

                if (gameObject.GetComponent<BoxCollider2D>().IsTouching(edgeRightColid.GetComponent<BoxCollider2D>()) || isRightEdgeTouched)
                {
                    rightMove = false;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }

                if (gameObject.GetComponent<BoxCollider2D>().IsTouching(cameraRightColid.GetComponent<BoxCollider2D>()))
                {
                    rightMove = false;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }

                if (gameObject.GetComponent<BoxCollider2D>().IsTouching(cameraLeftColid.GetComponent<BoxCollider2D>()))
                {
                    //This should make it collide with the left wall of the camera
                    gameObject.SetActive(false);
                    edgeWalkBack = 0;
                    gameObject.transform.position = startPosition;
                }
            }

            if (rightMove)
            {
                PatrolOpp();
            }
            else
            {
                Patrol();
            }
        }
    }

    void Patrol()
    {
       transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    void PatrolOpp()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }
}
