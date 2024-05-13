﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8;
    [SerializeField] private float jumpSpeed = 8;
    private Rigidbody2D _body;
    private Animator _anim;
    private bool _grounded;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        _body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, _body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector2(10, 10);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-10, 10);

        if (Input.GetKeyDown(KeyCode.UpArrow) && _grounded)
            Jump();

        _anim.SetBool("running", horizontalInput != 0);
    }

    private void Jump()
    {
        _anim.SetBool("jumping", true);
        _body.velocity = new Vector2(_body.velocity.x, jumpSpeed);
        _grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _anim.SetBool("jumping", false);
        _grounded = true;
    }
}