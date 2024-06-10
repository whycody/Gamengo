using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Image = UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private float speed = 8;
    [SerializeField] private float jumpSpeed = 8;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask deathLayer;

    [SerializeField] private GameObject gameManagerObject;

    [SerializeField] private Image staminaBar;
    [SerializeField] private float stamina = 500, maxStamina = 500;
    [SerializeField] private float jumpCost = 25, runCost = 25;
    [SerializeField] private float chargeRate = 20;

    private Coroutine _recharge;
    private GameManager gameManager;

    private float _defaultGravityScale;

    private readonly List<KeyCode> _jumpKeys = new List<KeyCode> { KeyCode.UpArrow, KeyCode.W, KeyCode.Space };
    private readonly List<KeyCode> _downKeys = new List<KeyCode> { KeyCode.DownArrow, KeyCode.S };

    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider;
    private Animator _anim;
    private Transform _originalParent;

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
        _defaultGravityScale = _body.gravityScale;
        stamina = maxStamina;
        UpdateStaminaBar();
        gameObject.tag = "Player";
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        chargeRate = _anim.GetBool(LayingParam) ? 40f : 20f;
        if (!gameManager.IsPaused && IsKilled())
            HandleDeath();
        if (!gameManager.IsPaused)
            Movement(horizontalInput, verticalInput);
        else
            _body.velocity = Vector3.zero;
    }

    private void HandleDeath()
    {
        gameManager.HandleDeath();
        ResetParent();
        gameObject.tag = "Untagged";
        ResetParams();
    }

    private void ResetParams()
    {
        _anim.SetBool(WalkingParam, false);
        _anim.SetBool(RunningParam, false);
        _anim.SetBool(JumpingParam, false);
    }

    private void Movement(float horizontalSpeed, float verticalSpeed)
    {
        #region Movement handling

        _body.velocity = new Vector2(horizontalSpeed * speed, _body.velocity.y);

        // turning right and left
        if (horizontalSpeed > 0.01f)
            transform.localScale = new Vector2(10, 10);
        else if (horizontalSpeed < -0.01f)
            transform.localScale = new Vector2(-10, 10);

        // Jumping handling
        if (_jumpKeys.Any(Input.GetKeyDown) && IsGrounded() && stamina >= jumpCost)
            Jump();

        if (_downKeys.Any(Input.GetKeyDown) && _anim.GetBool(IdleParam))
            _anim.SetBool(LayingParam, true);

        // Setting gravity force on Down Arrow
        _body.gravityScale = _downKeys.Any(Input.GetKey) ? 2 * _defaultGravityScale : _defaultGravityScale;

        speed = _anim.GetBool(RunningParam) ? 8 : 6;

        if (_anim.GetBool(RunningParam))
        {
            UseStamina(runCost);
            if (_recharge is not null) StopCoroutine(_recharge);
            _recharge = StartCoroutine(RechargeStamina());
        }

        jumpSpeed = _anim.GetBool(RunningParam) ? 9.5f : 8;

        #endregion

        #region Updating parameters

        // Updating Animator parameters
        UpdateJumpingParam();

        _anim.SetBool(RunningParam, Input.GetKey(KeyCode.LeftShift) && horizontalSpeed != 0 && stamina >= runCost);
        _anim.SetBool(WalkingParam, horizontalSpeed != 0);
        _anim.SetBool(IdleParam,
            !(_anim.GetBool(RunningParam) || _anim.GetBool(WalkingParam) || _anim.GetBool(JumpingParam)));
        if (!_anim.GetBool(IdleParam)) _anim.SetBool(LayingParam, false);

        #endregion
    }

    private void UpdateJumpingParam()
    {
        if (!(Math.Abs(_body.velocity.y) < 0.2) || !IsGrounded() || !_anim.GetBool(JumpingParam)) return;
        dust.Play();
        _anim.SetBool(JumpingParam, false);
    }

    private void Jump()
    {
        dust.Play();
        _anim.SetBool(JumpingParam, true);
        _anim.SetTrigger(JumpParam);
        _body.velocity = new Vector2(_body.velocity.x, jumpSpeed);
        UseStamina(jumpCost);
        if (_recharge is not null) StopCoroutine(_recharge);
        _recharge = StartCoroutine(RechargeStamina());
    }

    private bool IsGrounded()
    {
        var raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.5f,
            groundLayer);
        return raycastHit.collider is not null;
    }

    private bool IsKilled()
    {
        var raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f,
            deathLayer);
        return raycastHit.collider is not null;
    }

    public void SetParent(Transform newParent)
    {
        _originalParent = transform.parent;
        transform.parent = newParent;
    }

    public void ResetParent()
    {
        transform.parent = _originalParent;
    }

    private void UseStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0) stamina = 0;
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        if (staminaBar is not null)
        {
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (stamina < maxStamina)
        {
            stamina += chargeRate / 100f;
            if (stamina > maxStamina) stamina = maxStamina;
            UpdateStaminaBar();
            yield return new WaitForSeconds(0.01f);
        }
    }
}