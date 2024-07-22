using System;
using UnityEngine;

public class TowerBall : MonoBehaviour
{
    [SerializeField]
    private float _minMovementUnit;
    [SerializeField]
    private float _maxMovementUnit;
    [SerializeField]
    private float _movementSpeed = 1f;

    private Vector3 _startPosition;
    private Vector3 _movementDirection;
    private float _targetDistance;

    public event Action OnTargetReached;

    private void Start()
    {
        _startPosition = transform.position;
        _targetDistance = UnityEngine.Random.Range(_minMovementUnit, _maxMovementUnit);
    }

    public void SetShootDirection(Vector3 direction)
    {
        _movementDirection = direction.normalized;
    }

    private void Update()
    {
        MoveBall();
    }

    private void MoveBall()
    {
        transform.position += _movementDirection * _movementSpeed * Time.deltaTime;
        float distance = (transform.position - _startPosition).magnitude;
        if(distance > _targetDistance)
        {
            OnTargetReached?.Invoke();
        }
    }
}
