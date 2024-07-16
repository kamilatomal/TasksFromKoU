using System;
using UnityEngine;

public class GravityBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _ballRigidbody;
    [SerializeField]
    private Collider2D _ballCollider;

    private float _ballRadius;

    public Rigidbody2D BallRigidbody => _ballRigidbody;
    public Collider2D BallCollider => _ballCollider;

    public event Action<GravityBall, GravityBall, Vector3> OnCollisionBetweenBallsHappened;
    public event Action<GravityBall> OnBallDestroyed;

    private bool _isActive = true;

    private bool _onBallDestroyedCalled;

    private void Awake()
    {
        _ballRadius = transform.localScale.x / 2;
    }

    private void OnDestroy()
    {
        if(!_onBallDestroyedCalled)
        {
            OnBallDestroyed?.Invoke(this);
        }
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

    public float GetGravityBallMass()
    {
        return _ballRigidbody.mass;
    }

    public void SetRadius(float value)
    {
        transform.localScale = new Vector3(value * 2, value * 2, transform.localScale.z);
        _ballRadius = value;
    }

    public void SetMass(float value)
    {
        _ballRigidbody.mass = value;
    }

    public void DestroyBall()
    {
        OnBallDestroyed?.Invoke(this);
        _onBallDestroyedCalled = true;
        _isActive = false;
        Destroy(this.gameObject);
    }
}
