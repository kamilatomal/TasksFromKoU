using UnityEngine;

public class CradleBall : MonoBehaviour
{
    [SerializeField]
    private DistanceJoint2D _joint2D;
    [SerializeField]
    private Rigidbody2D _rigidBody2D;
    [SerializeField]
    private float _maxDragDistance = 6f;
    [SerializeField]
    private float _forceValue = 0.1f;

    private Vector2 _mouseWorldPosition;
    private Vector2 _jointPosition;
    private Vector2 _startBallPosition;
    private Vector2 _directionToTarget;
    private bool _isDragged;

    public Rigidbody2D RigidBody2D => _rigidBody2D;

    private void Awake()
    {
        _rigidBody2D.isKinematic = true;
        _startBallPosition = transform.position; 
        _joint2D.distance = _maxDragDistance;
        _jointPosition = _joint2D.connectedBody.transform.position;
        Vector2 direction = ((Vector2)transform.position - _jointPosition).normalized;
        transform.position = _jointPosition + (direction * _maxDragDistance);
    }

    private void FixedUpdate()
    {
        if (!_isDragged)
        {
            return;
        }

        else
        {
            float distanceBetweenMouseAndJoint = Vector2.Distance(_mouseWorldPosition, _joint2D.connectedBody.transform.position);
            if (distanceBetweenMouseAndJoint > _maxDragDistance)
            {
                Vector2 direction = (_mouseWorldPosition - _jointPosition).normalized;
                _rigidBody2D.MovePosition(_jointPosition + (direction * _maxDragDistance));
            }
            else
            {
                _rigidBody2D.MovePosition(_mouseWorldPosition);
            }
        }
    }
    private void OnMouseDown()
    {
        _isDragged = true;
    }

    private void OnMouseDrag()
    {
        _isDragged = true;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        _directionToTarget = (_startBallPosition - (Vector2)transform.position);
        _joint2D.enabled = true;
        _rigidBody2D.isKinematic = false;
        _isDragged = false;
        HandleBallBehaviourAfterRelease();
    }

    private void HandleBallBehaviourAfterRelease()
    {
        _rigidBody2D.AddForce(_directionToTarget * _forceValue, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        if (!_isDragged)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_joint2D.transform.position, _maxDragDistance);
        Vector2 direction = (_mouseWorldPosition - _jointPosition).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_jointPosition, _jointPosition + (direction * _maxDragDistance));
        Gizmos.color = Color.magenta;
    }
}
