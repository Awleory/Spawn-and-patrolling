using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour
{
    [SerializeField] private bool _isPlayer;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _gravityRate = 1f;
    [SerializeField] private LayerMask _collisionLayerMask;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private int _extraJumps = 0;
    [SerializeField] private float _minGroundNormalY = .65f;

    private ContactFilter2D _contactFilter;
    private Rigidbody2D _rigidBody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _jumped;
    private int _jumpsDone;
    private Vector2 _directionVelocity;
    private Vector2 _groundNormal;
    private bool _grounded;
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    private Vector2 _velocity;

    private const float _minMoveDistance = .001f;
    private const float _shellRadius = .01f;
    private const string _nameValueOfJumped = "jumped";
    private const string _nameValueOfSpeed = "speed";
    private const string _nameValueOfVelocityY = "velocityY";
    private const string _nameValueOfGrounded = "grounded";

    private void OnEnable()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_collisionLayerMask);
        _contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        if (_isPlayer)
        {
            GetInputByKeyBoard();
            UpdateAnimationParameters();
        }
    }

    private void FixedUpdate()
    {
        if (_directionVelocity.y != 0)
        {
            _velocity.y = _directionVelocity.y * _speed;
        }
        _velocity += _gravityRate * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _directionVelocity.x * _speed;

        _grounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 moveVector = moveAlongGround * deltaPosition.x;

        Move(moveVector, false);

        moveVector = Vector2.up * deltaPosition.y;

        Move(moveVector, true);
    }

    public void GiveMovement(Vector2 direction, bool jump = false)
    {
        _directionVelocity = direction;

        if (jump)
        {
            TryJump();
        }
    }

    private void GetInputByKeyBoard()
    {
        _directionVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }
    }

    private void TryJump()
    {
        if (_jumpsDone <= _extraJumps)
        {
            _jumped = true;
            _jumpsDone++;
            _velocity.y = _jumpForce;
        }
    }

    private void Move(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > _minMoveDistance)
        {
            RaycastHit2D[] _tempHitBuffer = new RaycastHit2D[16];

            int count = _rigidBody2D.Cast(move, _contactFilter, _tempHitBuffer, distance + _shellRadius);

            _hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                _hitBufferList.Add(_tempHitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    _grounded = true;
                    _jumped = false;
                    _jumpsDone = 0;

                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidBody2D.position = _rigidBody2D.position + move.normalized * distance;
    }

    private void UpdateAnimationParameters()
    {
        _animator.SetBool(_nameValueOfGrounded, _grounded);
        _animator.SetFloat(_nameValueOfVelocityY, _velocity.y);
        _animator.SetFloat(_nameValueOfSpeed, Mathf.Abs(_velocity.x));
        _animator.SetBool(_nameValueOfJumped, _jumped);
    }
}