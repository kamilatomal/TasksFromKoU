using System;
using UnityEngine;

public class SlingshotController : MonoBehaviour
{
    [SerializeField]
    private Ball _ballPrefab;
    private Ball _activeBall;
    public Ball Ball => _activeBall;

    public Action<Ball> OnActiveBallShootAction;
    public Action<Ball> OnActiveBallLandedAction;

    private void OnDisable()
    {
        if (_activeBall == null)
        {
            return;
        }
        _activeBall.OnBallLand -= OnBallLanded;
        _activeBall.OnBallLand -= OnBallShoot;
    }

    private void Start()
    {
        CreateBall();
    }

    private void CreateBall()
    {
        if (_activeBall != null)
        {
            _activeBall.OnBallLand -= OnBallLanded;
            _activeBall.OnBallLand -= OnBallShoot;
        }
        _activeBall = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        _activeBall.OnBallLand += OnBallLanded;
        _activeBall.OnBallShoot += OnBallShoot;
    }

    private void OnBallShoot()
    {
        OnActiveBallShootAction?.Invoke(_activeBall);
    }

    private void OnBallLanded()
    {
        OnActiveBallLandedAction?.Invoke(_activeBall);
        CreateBall();
    }
}
