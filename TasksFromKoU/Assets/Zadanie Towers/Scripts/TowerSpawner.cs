using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _towersContainer;

    private List<Tower> _towers = new List<Tower>();

    public List<Tower> Towers => _towers;
    public event Action OnBallSpawned;
    public event Action OnBallDestroyedAction;

    private void Start()
    {
        Tower spawnedTower = TowersPoolManager.Instance.GetBall();
        spawnedTower.transform.SetParent(_towersContainer);
        _towers.Add(spawnedTower);
    }
}
