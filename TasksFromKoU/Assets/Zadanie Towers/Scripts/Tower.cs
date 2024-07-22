using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Transform _ballSpawnerPoint;
    [SerializeField]
    private TowerBall _towerBallPrefab;
    [SerializeField]
    private List<SpriteRenderer> _bodySpriteRenderers;
    [SerializeField]
    private float minRotateDegrees;
    [SerializeField]
    private float maxRotateDegrees;
    [SerializeField]
    private float _towerRotateFrequency;
    [SerializeField]
    private float _maxShootAmount;
    [SerializeField]
    private float _untilActiveDelay;

    private float _timer = 0;
    private float _randomDegree;
    private float _counter;
    private bool _isActive;
    private bool _isDestroyed;
    private TowerSpawner _towerSpawner;
    private TowerBall _createdTowerBall;
    private Coroutine _activateCoroutine;

    public bool IsDestroyed { get { return _isDestroyed; } set { _isDestroyed = value; } }
    public event Action<Vector3> OnSpawnTowerAction;

    private void FixedUpdate()
    {
        if (!_isActive)
        {
            SetTowersColor();
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _towerRotateFrequency)
        {
            if (_counter >= _maxShootAmount)
            {
                _isActive = false;
                return;
            }
            RotateTower();
            Shoot();
            _counter += 1;
            _timer = 0;
        }
    }

    private void RotateTower()
    {
        _randomDegree = Random.Range(minRotateDegrees, maxRotateDegrees);
        transform.Rotate(new Vector3(0, 0, _randomDegree), Space.Self);
    }

    private void Shoot()
    {
        _createdTowerBall = Instantiate(_towerBallPrefab);
        _createdTowerBall.transform.position = _ballSpawnerPoint.transform.position;
        _createdTowerBall.Setup(_ballSpawnerPoint.transform.up, _towerSpawner, this);
    }

    private IEnumerator ActivateTowerCoroutine(float delay, float initialTimer)
    {
        yield return new WaitForSeconds(delay);
        _isActive = true;
        _counter = 0;
        _timer = initialTimer;
        SetTowersColor();
    }

    private void ActivateInternal(float delay, float initialTimer)
    {
        if (_activateCoroutine != null)
        {
            StopCoroutine(_activateCoroutine);
        }
        _activateCoroutine = StartCoroutine(ActivateTowerCoroutine(delay, initialTimer));
    }

    public void SetTowerSpawner(TowerSpawner towerSpawner)
    {
        _towerSpawner = towerSpawner;
    }

    public void Activate()
    {
        ActivateInternal(_untilActiveDelay, 0f);
    }



    public void SetTowersColor()
    {
        if (_isDestroyed)
        {
            return;
        }

        foreach (SpriteRenderer sprite in _bodySpriteRenderers)
        {
            sprite.color = _isActive ? Color.red : Color.white;
        }
    }

    public void ActivateTowerInstantly()
    {
        ActivateInternal(0, _towerRotateFrequency);
    }
}
