using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private Ball _ballPrefab;
    [SerializeField]
    private Rigidbody2D _body2D;

    private Ball _createdBall;
    private void Start()
    {
        CreateBall();
    }

    private void CreateBall()
    {
        _createdBall = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        _createdBall.SetAttachedRigidbody(_body2D);
    }
}
