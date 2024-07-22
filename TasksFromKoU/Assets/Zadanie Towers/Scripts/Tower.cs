using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Transform _ballSpawnerPoint;
    [SerializeField]
    private TowerBall _towerBallPrefab;
    [SerializeField]
    private float minRotateDegrees;
    [SerializeField]
    private float maxRotateDegrees;
    [SerializeField]
    private float _towerRotateFrequency;
    [SerializeField]
    private float _maxShootAmount;

    private float _timer = 0;
    private float _randomDegree;
    private float _counter;
    private TowerBall _createdTowerBall;

    public event Action OnSpawnTowerAction;

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        if(_timer >= _towerRotateFrequency)
        {
            RotateTower();
            _timer = 0;
        }
    }

    private void RotateTower()
    {
        if(_counter >= _maxShootAmount)
        {
            return;
        }
        _randomDegree = UnityEngine.Random.Range(minRotateDegrees, maxRotateDegrees);
        transform.Rotate(new Vector3(0, 0, _randomDegree), Space.Self);
        Shoot();
    }

    private void Shoot()
    {
        if (_createdTowerBall != null)
        {
            _createdTowerBall.OnTargetReached -= OnSpawnTower;
        }
        _createdTowerBall = Instantiate(_towerBallPrefab);
        _createdTowerBall.transform.position = _ballSpawnerPoint.transform.position;
        _createdTowerBall.SetShootDirection(_ballSpawnerPoint.transform.position);
        _counter += 1;
        _createdTowerBall.OnTargetReached += OnSpawnTower;
    }

    private void OnSpawnTower()
    {
        OnSpawnTowerAction?.Invoke();
    }
}
