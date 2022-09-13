using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MoveAnimation : MonoBehaviour
{
    [SerializeField] bool _flipX;

    private float _previousFrameX;
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _previousFrameX = transform.position.x;
    }

    private void Update()
    {
        if (_spriteRenderer.flipX == _flipX && transform.position.x < _previousFrameX
            || _spriteRenderer.flipX != _flipX && transform.position.x > _previousFrameX)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        _previousFrameX = transform.position.x;
    }
}
