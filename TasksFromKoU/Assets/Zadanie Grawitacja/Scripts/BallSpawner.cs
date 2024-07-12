using System;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private GravityBall _gravityBallPrefab;
    [SerializeField]
    private float _spawnFrequency = 0.25f;
    [SerializeField]
    private int _maxBallAmount = 250;

    private int _spawnedBallAmount = 0;
    private float _timer;

    public int SpawnedBallAmount => _spawnedBallAmount;
    public event Action OnBallSpawned;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnFrequency && _spawnedBallAmount < _maxBallAmount)
        {
            GravityBall createdBall = Instantiate(_gravityBallPrefab, transform.position, Quaternion.identity);
            _spawnedBallAmount += 1;
            OnBallSpawned?.Invoke();
            _timer = 0;
        }
    }
}
