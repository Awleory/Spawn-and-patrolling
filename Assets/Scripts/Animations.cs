using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class Animations : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _grounded;
    private bool _jumped;
    private float _minMoveDistance = .001f;
    private Vector2 _velocity;

    private void Start()
    {
        _animator.GetComponent<Animator>();
        _spriteRenderer.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        _velocity = transform.position;
        SetParameters();
    }

    private void SetParameters()
    {
        _animator.SetBool("grounded", _grounded);
        _animator.SetFloat("velocityY", _velocity.y);
        _animator.SetFloat("speed", Mathf.Abs(_velocity.x));
        _animator.SetBool("jumped", _jumped);

        if (_spriteRenderer.flipX == false && _velocity.x < -_minMoveDistance || _spriteRenderer.flipX && _velocity.x > _minMoveDistance)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
    }
}
