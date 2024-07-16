using System;
using System.Collections;
using UnityEngine;

public class GravityBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _ballRigidbody;
    [SerializeField]
    private Collider2D _ballCollider;

    private float _ballRadius;
    private float _originalBallRadius;
    private bool _isActive = false;
    private Coroutine _activationCoroutine;

    public Rigidbody2D BallRigidbody => _ballRigidbody;
    public event Action<GravityBall, GravityBall, Vector3> OnCollisionBetweenBallsHappened;
    public event Action<GravityBall> OnBallDestroyed;
    public bool IsActive => _isActive;

    private void Awake()
    {
        _originalBallRadius = transform.localScale.x / 2;
        ResetBall(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!_isActive)
        {
            return;
        }
        GravityBall otherGravityBall = collision.collider.GetComponent<GravityBall>();
        if (otherGravityBall == null || !otherGravityBall._isActive)
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

    public void ActivateWithDelay(float delay)
    {
        if(_activationCoroutine != null)
        {
            StopCoroutine(_activationCoroutine);
        }
        _activationCoroutine = StartCoroutine(ActivateWithDelayCoroutine(delay));
    }

    public IEnumerator ActivateWithDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isActive = true;
        _activationCoroutine = null;
        _ballCollider.enabled = true;
    }

    public void ResetBall(bool invokeDestroyEvent)
    {
        if(invokeDestroyEvent)
        { 
            OnBallDestroyed?.Invoke(this);
        }
        _isActive = false;
        SetRadius(_originalBallRadius);
        _ballCollider.enabled = false;
        if(_activationCoroutine != null)
        {
            StopCoroutine(_activationCoroutine);
        }
    }
}
