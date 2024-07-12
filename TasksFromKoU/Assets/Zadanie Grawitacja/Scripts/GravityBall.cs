using UnityEngine;

public class GravityBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _ballRigidbody;

    public Rigidbody2D BallRigidbody => _ballRigidbody;
}
