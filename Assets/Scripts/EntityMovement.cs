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
    private float jumpHight;
    public GameObject player;
    private float speed;
    private int edgeWalkBack = 0;
    private bool rightMove;
    private new Rigidbody2D rigidbody;

    private Vector3 startPosition;
    private bool movingLeft = true;

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

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (cameraLeftColid != null)
        {
            if (gameObject.GetComponent<BoxCollider2D>().IsTouching(edgeRightColid.GetComponent<BoxCollider2D>()))
            {
                if (edgeWalkBack == 0)
                {
                    rightMove = true;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    edgeWalkBack = 1;
                }
                if(edgeWalkBack == 1)
                {
                    Rigidbody2D EnemyRid = gameObject.GetComponent<Rigidbody2D>();
                    EnemyRid.AddForce(Vector3.up * jumpHight);
                }
            }

            if (gameObject.GetComponent<BoxCollider2D>().IsTouching(edgeLeftColid.GetComponent<BoxCollider2D>()))
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

    private void PlayerCollision()
    {
        var playerBody = player.GetComponent<Rigidbody2D>();

    }
}
