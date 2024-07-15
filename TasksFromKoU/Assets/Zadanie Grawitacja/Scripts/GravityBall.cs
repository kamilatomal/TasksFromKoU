using System;
using UnityEngine;

public class GravityBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _ballRigidbody;

    public Rigidbody2D BallRigidbody => _ballRigidbody;
    private float _ballRadius;

    public event Action<GravityBall, GravityBall, Vector3> OnCollisionBetweenBallsHappened;
    public event Action<GravityBall> OnBallDestroyed;

    private bool _isActive = true;

    private void Awake()
    {
        _ballRadius = transform.localScale.x / 2;
    }

    private void OnDestroy()
    {
        OnBallDestroyed?.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!_isActive)
        {
            return;
        }
        GravityBall otherGravityBall = collision.collider.GetComponent<GravityBall>();
        if (otherGravityBall == null)
        {
            return;
        }
        
        OnCollisionBetweenBallsHappened?.Invoke(this, otherGravityBall, collision.contacts[0].point);
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

    public void DestroyBall()
    {
        _isActive = false;
        Destroy(this.gameObject);
    }
}
