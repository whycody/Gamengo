using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [SerializeField] private float speed = 8;
    [SerializeField] private float jumpSpeed = 8;
    [SerializeField] private LayerMask groundLayer;

    private readonly List<KeyCode> _jumpKeys = new List<KeyCode> { KeyCode.UpArrow, KeyCode.W, KeyCode.Space };
    private readonly List<KeyCode> _downKeys = new List<KeyCode> { KeyCode.DownArrow, KeyCode.S };

    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider;
    private Animator _anim;

    #endregion

    #region Parameters

    // many people say that's a lot faster to give int to getBool() instead of string
    private static readonly int RunningParam = Animator.StringToHash("running");
    private static readonly int WalkingParam = Animator.StringToHash("walking");
    private static readonly int JumpingParam = Animator.StringToHash("jumping");
    private static readonly int JumpParam = Animator.StringToHash("jump");
    private static readonly int IdleParam = Animator.StringToHash("idle");
    private static readonly int LayingParam = Animator.StringToHash("laying");
    //private static readonly int GroundedParam = Animator.StringToHash("grounded");

    #endregion


    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        #region Movement handling

        var horizontalInput = Input.GetAxis("Horizontal");
        _body.velocity = new Vector2(horizontalInput * speed, _body.velocity.y);

        // turning right and left
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector2(10, 10);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector2(-10, 10);

        // Jumping handling
        if (_jumpKeys.Any(Input.GetKeyDown) && IsGrounded())
            Jump();

        if (_downKeys.Any(Input.GetKeyDown) && _anim.GetBool(IdleParam))
            _anim.SetBool(LayingParam, true);

        // Setting gravity force on Down Arrow
        _body.gravityScale = _downKeys.Any(Input.GetKey) ? 4f : 2f;


        speed = _anim.GetBool(RunningParam) ? 8 : 6;

        jumpSpeed = _anim.GetBool(RunningParam) ? 9 : 8;

        #endregion

        #region Updating parameters

        // Updating Animator parameters
        _anim.SetBool(RunningParam, Input.GetKey(KeyCode.LeftShift) && horizontalInput != 0);
        _anim.SetBool(WalkingParam, horizontalInput != 0);
        _anim.SetBool(IdleParam,
            !(_anim.GetBool(RunningParam) || _anim.GetBool(WalkingParam) || _anim.GetBool(JumpingParam)));
        if(!_anim.GetBool(IdleParam)) _anim.SetBool(LayingParam, false);
        
        #endregion
        
        print(IsGrounded());
    }

    private void Jump()
    {
        _anim.SetBool(JumpingParam, true);
        _anim.SetTrigger(JumpParam);
        _body.velocity = new Vector2(_body.velocity.x, jumpSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsGrounded()) return;
        _anim.SetBool(JumpingParam, false);
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider is not null;
    }
}