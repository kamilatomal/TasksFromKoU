using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private Ball _ballPrefab;

    private Ball _createdBall;
    private void Start()
    {
        CreateBall();
    }

    private void CreateBall()
    {
        _createdBall = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
    }
}
