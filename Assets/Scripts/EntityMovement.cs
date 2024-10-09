using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    private GameObject cameraLeftColid;
    public GameObject player;
    public float speed = 2f;
    public float patrolDistance = 5f;

    private Vector3 startPosition;
    private bool movingLeft = true;

    public void setCameraColid(ref GameObject extraCameraLeftColid)
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
            if (gameObject.GetComponent<BoxCollider2D>().IsTouching(cameraLeftColid.GetComponent<BoxCollider2D>()))
            {
                //This should make it collide with the left wall of the camera
                gameObject.SetActive(false);
                gameObject.transform.position = startPosition;
            }
        }
        Patrol();
    }

    void Patrol()
    {
       transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    //public float speed = 1f;
    //public Vector2 direction = Vector2.left;

    private new Rigidbody2D rigidbody;
    //private Vector2 velocity;

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


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    collision.enabled = true;
    //    Debug.Log(collision);
    //}

    //private void Update()
    //{
    //    velocity.x = direction.x * speed;
    //    velocity.y = Physics2D.gravity.y;

    //    rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
    //    if(rigidbody.Raycast(direction))
    //    {
    //        direction = -direction;
    //    }
    //}

}
