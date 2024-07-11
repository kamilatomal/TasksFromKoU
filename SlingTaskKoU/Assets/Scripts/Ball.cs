using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private TargetJoint2D _springJoint;
    [SerializeField]
    private Rigidbody2D _rigidBody2D;
    [SerializeField]
    private float _maxBallDistance;
    [SerializeField]
    private float _releaseDelay;

    private Vector2 _mouseWorldPosition;

    private bool _isDragged;

    public void SetAttachedRigidbody(Rigidbody2D attachedRigidbody)
    {
        _springJoint.connectedBody = attachedRigidbody;
    }

    private void Awake()
    {
        _releaseDelay = 1 / (_springJoint.frequency * 2);
    }

    private void FixedUpdate()
    {
        if(!_isDragged)
        {
            return;
        }
        _rigidBody2D.MovePosition(_mouseWorldPosition);
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
        _springJoint.enabled = true;
        _rigidBody2D.isKinematic = false;
        _isDragged = false;
        StartCoroutine(Release());
    }

    private IEnumerator Release()
    {
        yield return new WaitForSeconds(_releaseDelay);
        _springJoint.enabled = false;
    }
}
