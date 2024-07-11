using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private Ball _ballPrefab;

    private Ball _createdBall;

    private void OnDisable()
    {
        if (_createdBall == null)
        {
            return;
        }
        _createdBall.OnBallLand -= CreateBall;
    }

    private void Start()
    {
        CreateBall();
    }

    private void CreateBall()
    {
        if (_createdBall != null)
        {
            _createdBall.OnBallLand -= CreateBall;
        }
        _createdBall = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        _createdBall.OnBallLand += CreateBall;
    }
}
