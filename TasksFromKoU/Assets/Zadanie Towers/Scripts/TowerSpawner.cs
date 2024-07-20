using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private int _maxBallAmount = 250;
    [SerializeField]
    private Transform _ballContainer;
    [SerializeField]
    private float _maxBallMass = 50;
    [SerializeField]
    private float _forceValue = 0.1f;

    private List<TowerBall> _towerBalls = new List<TowerBall>();

    public List<TowerBall> TowerBalls => _towerBalls;
    public event Action OnBallSpawned;
    public event Action OnBallDestroyedAction;
}
