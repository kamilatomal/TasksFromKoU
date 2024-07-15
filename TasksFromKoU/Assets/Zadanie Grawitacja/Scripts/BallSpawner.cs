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
    private Vector2 _spawnRandomPosition;
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

    private void SpawnGravityBall(float? radius = null)
    {
        _spawnRandomPosition = new Vector2(UnityEngine.Random.Range(_minCameraX, _maxCameraX), UnityEngine.Random.Range(_minCameraY, _maxCameraY));
        GravityBall spawnedBall = Instantiate(_gravityBallPrefab, _spawnRandomPosition, Quaternion.identity);
        if(radius.HasValue)
        {
            spawnedBall.SetRadius(radius.Value);
        }
        spawnedBall.transform.SetParent(_ballContainer);
        _gravityBalls.Add(spawnedBall);
        spawnedBall.OnCollisionBetweenBallsHappened += SetupCreatedBall;
        OnBallSpawned?.Invoke();
    }

    private float GetNewGravityBallRadious(GravityBall ballA, GravityBall ballB)
    {
        float newBallArea = ballA.GetGravityBallArea() + ballB.GetGravityBallArea();
        return (float)Math.Sqrt(newBallArea / Math.PI);
    }

    private void SetupCreatedBall(Collision2D collision, GravityBall ballA, GravityBall ballB)
    {
        float newBallScale = GetNewGravityBallRadious(ballA, ballB);
        SpawnGravityBall(newBallScale);
    }
}
