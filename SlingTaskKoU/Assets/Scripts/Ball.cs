using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private TargetJoint2D _springJoint;
    [SerializeField]
    private Rigidbody2D _rigidBody2D;
    [SerializeField]
    private float _maxDragDistance = 6f;
    [SerializeField]
    private float _minBallShootDistance = 1f;
    [SerializeField]
    private float _forceValue = 0.1f;

    private Vector2 _mouseWorldPosition;
    private Vector3 _targetPosition;
    private Vector3 _startDirectionToTarget;
    private bool _isDragged;

    public event Action OnBallLand;

    private void Awake()
    {
        _springJoint.target = new Vector2(transform.position.x, transform.position.y);
        _targetPosition = new Vector3(_springJoint.target.x, _springJoint.target.y, 0);
    }

    private void FixedUpdate()
    {
        if (!_isDragged)
        {
            return;
        }

        else
        {
            float distanceBetweenMouseAndJoint = Vector2.Distance(_mouseWorldPosition, _springJoint.target);
            if (distanceBetweenMouseAndJoint > _maxDragDistance)
            {
                Vector2 direction = (_mouseWorldPosition - _springJoint.target).normalized;
                _rigidBody2D.MovePosition(_springJoint.target + (direction * _maxDragDistance));
            }

            else
            {
                _rigidBody2D.MovePosition(_mouseWorldPosition);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnBallLand?.Invoke();
    }

    private void OnMouseDown()
    {
        _isDragged = true;
        _springJoint.enabled = false;
        _rigidBody2D.isKinematic = true;
    }

    private void OnMouseDrag()
    {
        _isDragged = true;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        _startDirectionToTarget = (_targetPosition - transform.position);
        _rigidBody2D.isKinematic = false;
        _isDragged = false;
        HandleBallBehaviourAfterRelease();
    }

    private void HandleBallBehaviourAfterRelease()
    {
        float distanceBetweenJointAndBall = Vector2.Distance(_springJoint.target, transform.position);
        if (distanceBetweenJointAndBall < _minBallShootDistance)
        {
            _springJoint.enabled = true;
        }

        else
        {
            _springJoint.enabled = false;
            _rigidBody2D.AddForce(_startDirectionToTarget * _forceValue, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        if(!_isDragged)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_targetPosition, _maxDragDistance);
        Vector2 direction = (_mouseWorldPosition - _springJoint.target).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_springJoint.target, _springJoint.target + (direction * _maxDragDistance));
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_targetPosition, _minBallShootDistance);
    }
}
