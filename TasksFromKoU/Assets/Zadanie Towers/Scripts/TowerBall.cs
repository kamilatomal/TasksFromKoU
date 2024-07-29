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
    private TowerSpawner _towerSpawner;
    private Tower _creator;
    private bool _isBallDestroyed;
 
    private void Start()
    {
        _startPosition = transform.position;
        _targetDistance = Random.Range(_minMovementUnit, _maxMovementUnit);
        _isBallDestroyed = false;
    }

    private void Update()
    {
        MoveBall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.TryGetComponent(out Tower tower))
        {
            if (tower == null || tower == _creator)
            {
                return;
            }
            else
            {
                OnDestroyObjectsBehavior(tower);
            }
        }
    }

    private void OnDestroyObjectsBehavior(Tower tower)
    {
        _isBallDestroyed = true;
        tower.IsDestroyed = true;
        _towerSpawner.OnTowerDestroyed(tower);
        TowersPoolManager.Instance.ReturnTowerBackToPool(tower);
        Destroy(gameObject);
    }

    public void Setup(Vector3 direction, TowerSpawner towerSpawner, Tower creator)
    {
        _movementDirection = direction.normalized;
        _towerSpawner = towerSpawner;
        _creator = creator;
    }

    private void MoveBall()
    {
        transform.position += _movementDirection * _movementSpeed * Time.deltaTime;
        float distance = (transform.position - _startPosition).magnitude;
        if(distance >= _targetDistance && !_isBallDestroyed)
        {
            Destroy(gameObject);
            _isBallDestroyed = true;
            if (_towerSpawner.CanSpawn)
            {
                _towerSpawner.CreateTower(transform.position, false);
            }
        }
    }
}
