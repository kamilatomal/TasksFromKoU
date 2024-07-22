using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _towersContainer;

    private List<Tower> _towers = new List<Tower>();
    private Tower _createdTower;

    public List<Tower> Towers => _towers;
    public event Action OnTowerSpawned;
    public event Action OnTowerDestroyedAction;

    private void Start()
    {
        CreateTower();
    }

    private void CreateTower()
    {
        if (_createdTower != null)
        {
            _createdTower.OnSpawnTowerAction -= CreateTower;
        }

        _createdTower = TowersPoolManager.Instance.GetBall();
        _createdTower.transform.SetParent(_towersContainer);
        _towers.Add(_createdTower);
        _createdTower.OnSpawnTowerAction += CreateTower;
        OnTowerSpawned?.Invoke();
    }
}
