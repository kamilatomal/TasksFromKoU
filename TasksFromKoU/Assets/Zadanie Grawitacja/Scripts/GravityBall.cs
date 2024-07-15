using System;
using UnityEngine;

public class GravityBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _ballRigidbody;

    public Rigidbody2D BallRigidbody => _ballRigidbody;
    private float _ballRadius;

    public event Action<Collision2D, GravityBall, GravityBall> OnCollisionBetweenBallsHappened;

    private void Awake()
    {
        _ballRadius = transform.localScale.x / 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GravityBall otherGravityBall = collision.collider.GetComponent<GravityBall>();
        if (otherGravityBall == null)
        {
            return;
        }
        OnCollisionBetweenBallsHappened?.Invoke(collision, this, otherGravityBall);
        Destroy(this.gameObject);
        Destroy(otherGravityBall.gameObject);
    }

    public float GetGravityBallArea()
    {
        return (float)(Math.PI * Math.Pow(_ballRadius, 2));
    }

    public void SetRadius(float value)
    {
        transform.localScale = new Vector3(value * 2, value * 2, transform.localScale.z);
        _ballRadius = value;
    }
}
