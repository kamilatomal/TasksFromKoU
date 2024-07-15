using System;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private GravityBall _gravityBallPrefab;
    [SerializeField]
    private float _spawnFrequency = 0.25f;
    [SerializeField]
    private int _maxBallAmount = 250;
    [SerializeField]
    private Transform _ballContainer;

    private float _timer;
    private float _minCameraX;
    private float _minCameraY;
    private float _maxCameraX;
    private float _maxCameraY;
    private List<GravityBall> _gravityBalls = new List<GravityBall>();

    public List<GravityBall> GravityBalls => _gravityBalls;
    public event Action OnBallSpawned;

    private void Start()
    {
        SetBounds();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnFrequency && _gravityBalls.Count < _maxBallAmount)
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

    private void SpawnGravityBall(float? radius = null, Vector3? spawnPosition = null)
    {
        Vector3 ballPosition;
        if (spawnPosition.HasValue)
        {
            ballPosition = spawnPosition.Value;
        }
        else
        {
            ballPosition = new Vector2(UnityEngine.Random.Range(_minCameraX, _maxCameraX), UnityEngine.Random.Range(_minCameraY, _maxCameraY));
        }

        GravityBall spawnedBall = Instantiate(_gravityBallPrefab, ballPosition, Quaternion.identity);

        if (radius.HasValue)
        {
            spawnedBall.SetRadius(radius.Value);
        }

        spawnedBall.transform.SetParent(_ballContainer);
        _gravityBalls.Add(spawnedBall);
        spawnedBall.OnCollisionBetweenBallsHappened += OnBallsColision;
        spawnedBall.OnBallDestroyed += OnBallDestroyed;
        OnBallSpawned?.Invoke();
    }

    private float GetNewGravityBallRadius(GravityBall ballA, GravityBall ballB)
    {
        float newBallArea = ballA.GetGravityBallArea() + ballB.GetGravityBallArea();
        return (float)Math.Sqrt(newBallArea / Math.PI);
    }

    private void OnBallsColision(GravityBall ballA, GravityBall ballB, Vector3 spawnPosition)
    {
        float newBallScale = GetNewGravityBallRadius(ballA, ballB);
        ballA.DestroyBall();
        ballB.DestroyBall();
        SpawnGravityBall(newBallScale, spawnPosition);
    }

    private void OnBallDestroyed(GravityBall gravityBall)
    {
        gravityBall.OnCollisionBetweenBallsHappened -= OnBallsColision;
        gravityBall.OnBallDestroyed -= OnBallDestroyed;
        _gravityBalls.Remove(gravityBall); 
    }
}
