/***************************************************************
*file: DoubleIt.cs
*author: Sean Butler
*author: Ahmad Alkadi
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/6/2024
*
*purpose: Charactar movement and condition of what the player can do
*
*References:
*Creator name: Pandemonium, link of the video that was used: https://www.youtube.com/watch?v=PUpC44Q64zY&list=PLgOEwFbvGm5o8hayFB6skAfa8Z-mw4dPV&index=4
*Creator name: AdamCYounis, link of the video that was used: https://www.youtube.com/watch?v=0-c3ErDzrh8
*
****************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpSpeed = 5;

    [Range(0f, 1f)]
    public float drag;
    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;


    public bool grounded;

    float xInput;
    float yInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Movement();
        Jump();
    }

    void Movement()
    {
        if (Mathf.Abs(xInput) > 0)
        {
            body.velocity = new Vector2(xInput * speed, body.velocity.y);
        }
    }

    void Jump()
    {
        if (yInput > 0 && grounded)
        {
            if (body.velocity.y <= 0.001 && body.velocity.y >= 0)
            {
                body.velocity = new Vector2(xInput, jumpSpeed);
            }
        }
    }

    void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        CheckGround();

        if(grounded && xInput == 0 && yInput == 0) { body.velocity *= drag; }
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max,groundMask).Length > 0;
    }

    public bool canAttak()
    {
        return true;
    }
}
