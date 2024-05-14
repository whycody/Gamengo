using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8;
    [SerializeField] private float jumpSpeed = 8;
    private Rigidbody2D _body;
    private Animator _anim;
    private bool _grounded;
    
    // many people say that's a lot faster to give int to getBool() instead of string
    private static readonly int RunningParam = Animator.StringToHash("running");
    private static readonly int WalkingParam = Animator.StringToHash("walking");
    private static readonly int JumpingParam = Animator.StringToHash("jumping");
    private static readonly int JumpParam = Animator.StringToHash("jump");
    private static readonly int GroundedParam = Animator.StringToHash("grounded");


    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");
        _body.velocity = new Vector2(horizontalInput * speed, _body.velocity.y);

        // turning right and left
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector2(10, 10);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-10, 10);
        
        // Jumping handling
        if (Input.GetKeyDown(KeyCode.UpArrow) && _grounded)
            Jump();
        
        // Setting gravity force on Down Arrow
        _body.gravityScale = Input.GetKey(KeyCode.DownArrow) ? 4f : 1.5f;

        speed = _anim.GetBool(RunningParam) ? 8 : 6;

        // Updating Animator parameters
        _anim.SetBool(RunningParam, Input.GetKey(KeyCode.LeftShift) && horizontalInput != 0);
        _anim.SetBool(WalkingParam, horizontalInput != 0);
    }

    private void Jump()
    {
        _anim.SetBool(JumpingParam, true);
        _anim.SetTrigger(JumpParam);
        _body.velocity = new Vector2(_body.velocity.x, jumpSpeed);
        _grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        _anim.SetBool(JumpingParam, false);
        _grounded = true;
    }
}
