using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _towersContainer;
    [SerializeField]
    private float _maxTowersAmount = 100f;

    private bool _canSpawn = false;
    private List<Tower> _towers = new List<Tower>();

    public List<Tower> Towers => _towers;
    public bool CanSpawn => _canSpawn;
    public event Action OnTowerSpawned;
    public event Action OnTowerDestroyedAction;

    private void Start()
    {
        CreateTower(transform.position, true);
    }

    public void CreateTower(Vector3 spawnPosition, bool isActive)
    {
        _canSpawn = true;
        Tower createdTower = TowersPoolManager.Instance.GetTower();
        createdTower.SetTowerSpawner(this);
        createdTower.transform.SetParent(_towersContainer);
        createdTower.transform.position = spawnPosition;
        _towers.Add(createdTower);
        createdTower.SetTowersColor();
        OnTowerSpawned?.Invoke();
        if (isActive)
        {
            createdTower.ActivateTowerInstantly();
        }
        else
        {
            createdTower.Activate();
        }

        if (_towers.Count >= _maxTowersAmount)
        {
            foreach (Tower tower in _towers)
            {
                tower.ActivateTowerInstantly();
                _canSpawn = false;
            }
        }
    }

    public void OnTowerDestroyed(Tower tower)
    {
        _towers.Remove(tower);
        OnTowerDestroyedAction?.Invoke();
    }
}
