using UnityEngine;

public class CradleBall : MonoBehaviour
{
    [SerializeField]
    private DistanceJoint2D _joint2D;
    [SerializeField]
    private Rigidbody2D _rigidBody2D;

    private Vector2 _mouseWorldPosition;
    private Vector2 _jointConnectedAnchor;
    private Vector2 _mouseOffset;
    private Vector2 _mouseOffsetWithMousePosition;
    private bool _isDragged;

    private void Awake()
    {
        _jointConnectedAnchor = _joint2D.connectedBody.position + _joint2D.connectedAnchor;
    }

    private void FixedUpdate()
    {
        if (!_isDragged)
        {
            return;
        }

        else
        {
            float distanceBetweenMouseAndJoint = Vector2.Distance(_mouseOffsetWithMousePosition + _joint2D.anchor, _jointConnectedAnchor);
            if (distanceBetweenMouseAndJoint > _joint2D.distance)
            {
                Vector2 direction = ((_mouseOffsetWithMousePosition + _joint2D.anchor) - _jointConnectedAnchor).normalized;
                _rigidBody2D.MovePosition(_jointConnectedAnchor + (direction * _joint2D.distance) - _joint2D.anchor);
            }
            else
            {
                _rigidBody2D.MovePosition(_mouseOffsetWithMousePosition);
            }
        }
    }

    private void OnMouseDown()
    {
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseOffset = _rigidBody2D.position - _mouseWorldPosition;
    }

    private void OnMouseDrag()
    {
        _isDragged = true;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseOffsetWithMousePosition = _mouseWorldPosition + _mouseOffset;
    }

    private void OnMouseUp()
    {
        _isDragged = false;
    }

    private void OnDrawGizmos()
    {
        if (!_isDragged)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_joint2D.transform.position, _joint2D.distance);
        Vector2 direction = (_mouseWorldPosition - _jointConnectedAnchor).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_jointConnectedAnchor, _jointConnectedAnchor + (direction * _joint2D.distance));
    }
}
