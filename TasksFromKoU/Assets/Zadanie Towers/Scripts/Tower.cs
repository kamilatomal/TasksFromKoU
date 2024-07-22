using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Transform _smallerBody;
    [SerializeField]
    private TowerBall _towerBallPrefab;
    [SerializeField]
    private Transform _towerBallsContainer;
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
        _randomDegree = Random.Range(minRotateDegrees, maxRotateDegrees);
        transform.Rotate(new Vector3(0, 0, _randomDegree), Space.Self);
        Shoot();
    }

    private void Shoot()
    {
        _createdTowerBall = Instantiate(_towerBallPrefab);
        _createdTowerBall.transform.SetParent(_towerBallsContainer);
        _createdTowerBall.transform.position = _smallerBody.transform.position;
        _counter += 1;
    }
}
