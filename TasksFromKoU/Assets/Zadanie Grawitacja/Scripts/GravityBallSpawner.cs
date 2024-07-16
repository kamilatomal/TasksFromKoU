using System;
using System.Collections.Generic;
using UnityEngine;

public class GravityBallSpawner : MonoBehaviour
{
    [SerializeField]
    private float _spawnFrequency = 0.25f;
    [SerializeField]
    private int _maxBallAmount = 250;
    [SerializeField]
    private Transform _ballContainer;
    [SerializeField]
    private float _maxBallMass = 50;
    [SerializeField]
    private float _forceValue = 0.1f;
    [SerializeField]
    private float _turnOnColliderDelay = 0.5f;

    private float _timer;
    private float _minCameraX;
    private float _minCameraY;
    private float _maxCameraX;
    private float _maxCameraY;
    private float _newBallMass;
    private float _minBallMass = 1f;
    private float _defaultRadius = 0.5f;
    private Vector3 _newBallPosition;
    private List<GravityBall> _gravityBalls = new List<GravityBall>();
    private ForceType _forceType;

    public List<GravityBall> GravityBalls => _gravityBalls;
    public ForceType ForceType => _forceType;
    public event Action OnBallSpawned;
    public event Action OnBallDestroyedAction;

    private void Start()
    {
        _forceType = ForceType.Attract;
        SetBounds();
    }

    private void OnEnable()
    {
        OnBallSpawned += OnBallAmountChanged;
        OnBallDestroyedAction += OnBallAmountChanged;
    }

    private void OnDisable()
    {
        OnBallSpawned -= OnBallAmountChanged;
        OnBallDestroyedAction -= OnBallAmountChanged;
    }

    private void Update()
    {
        if (_forceType == ForceType.Repel)
        {
            return;
        }
        _timer += Time.deltaTime;
        if (_timer >= _spawnFrequency)
        {
            SpawnGravityBall();
            _timer = 0;
        }
    }

    private void SetBounds()
    {
        Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector3 bottomLeftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        _minCameraX = bottomLeftCorner.x;
        _maxCameraX = upperRightCorner.x;
        _minCameraY = bottomLeftCorner.y;
        _maxCameraY = upperRightCorner.y;
    }

    private GravityBall SpawnGravityBall(float? radius = null, Vector3? spawnPosition = null, float? mass = null, float turnOnColliderDelay = 0)
    {
        if (_gravityBalls.Count >= _maxBallAmount)
        {
            return null;
        }

        Vector3 ballPosition;
        if (spawnPosition.HasValue)
        {
            ballPosition = spawnPosition.Value;
        }
        else
        {
            ballPosition = new Vector2(UnityEngine.Random.Range(_minCameraX, _maxCameraX), UnityEngine.Random.Range(_minCameraY, _maxCameraY));
        }

        GravityBall spawnedBall = PoolManager.Instance.GetBall();
        
        if (radius.HasValue)
        {
            spawnedBall.SetRadius(radius.Value);
        }

        if (mass.HasValue)
        {
            spawnedBall.SetMass(mass.Value);
        }

        spawnedBall.transform.SetParent(_ballContainer);
        spawnedBall.transform.position = ballPosition;
        _gravityBalls.Add(spawnedBall);
        spawnedBall.OnCollisionBetweenBallsHappened += OnBallsColision;
        spawnedBall.OnBallDestroyed += OnBallDestroyed;
        OnBallSpawned?.Invoke();
        spawnedBall.ActivateWithDelay(turnOnColliderDelay);
        return spawnedBall;
    }

    private float GetNewGravityBallRadius(GravityBall ballA, GravityBall ballB)
    {
        float newBallArea = ballA.GetGravityBallArea() + ballB.GetGravityBallArea();
        return (float)Math.Sqrt(newBallArea / Math.PI);
    }

    private float GetNewGravityBallMass(GravityBall ballA, GravityBall ballB)
    {
        float newBallMass = ballA.GetGravityBallMass() + ballB.GetGravityBallMass();
        return newBallMass;
    }

    private void OnBallsColision(GravityBall ballA, GravityBall ballB, Vector3 spawnPosition)
    {
        if (_forceType == ForceType.Repel)
        {
            return;
        }

        float newBallScale = GetNewGravityBallRadius(ballA, ballB);
        _newBallMass = GetNewGravityBallMass(ballA, ballB);
        _newBallPosition = spawnPosition;
        PoolManager.Instance.ReturnBallBackToPool(ballA);
        PoolManager.Instance.ReturnBallBackToPool(ballB);
        if (_newBallMass >= _maxBallMass)
        {
            BallExplosion();
            return;
        }
        SpawnGravityBall(newBallScale, spawnPosition, _newBallMass);
    }

    private void OnBallDestroyed(GravityBall gravityBall)
    {
        gravityBall.OnCollisionBetweenBallsHappened -= OnBallsColision;
        gravityBall.OnBallDestroyed -= OnBallDestroyed;
        _gravityBalls.Remove(gravityBall);
        OnBallDestroyedAction?.Invoke();
    }

    private void BallExplosion()
    {
        for (int i = 0; i < _maxBallMass; i++)
        {
            GravityBall gravityBall = SpawnGravityBall(_defaultRadius, _newBallPosition, _minBallMass, _turnOnColliderDelay);
            if(gravityBall == null)
            {
                break;
            }
            float randomX = UnityEngine.Random.Range(-1f, 1f);
            float randomY = UnityEngine.Random.Range(-1f, 1f);
            gravityBall.BallRigidbody.AddForce(new Vector2(randomX, randomY).normalized * _forceValue, ForceMode2D.Impulse);
        }
    }

    private void OnBallAmountChanged()
    {
        if (_gravityBalls.Count >= _maxBallAmount)
        {
            _forceType = ForceType.Repel;
        }
    }
}
