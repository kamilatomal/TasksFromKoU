using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _towersContainer;

    private List<Tower> _towers = new List<Tower>();
    public List<Tower> Towers => _towers;
    public event Action OnTowerSpawned;
    public event Action OnTowerDestroyedAction;

    private void Start()
    {
        CreateTower(transform.position, true);
    }

    public void CreateTower(Vector3 spawnPosition, bool isActive)
    {
        Tower createdTower = TowersPoolManager.Instance.GetBall();
        createdTower.SetTowerSpawner(this);
        createdTower.transform.SetParent(_towersContainer);
        createdTower.transform.position = spawnPosition;
        _towers.Add(createdTower);
        createdTower.IsActive = isActive;
        createdTower.SetTowersColor();
        OnTowerSpawned?.Invoke();
        StartCoroutine(ActivateTower(createdTower));
    }

    private IEnumerator ActivateTower(Tower tower)
    {
        yield return new WaitForSeconds(tower.UntilActiveDelay);
        tower.IsActive = true;
        tower.SetTowersColor();
    }

    public void OnTowerDestroyed(Tower tower)
    {
        _towers.Remove(tower);
        OnTowerDestroyedAction?.Invoke();
    }
}
