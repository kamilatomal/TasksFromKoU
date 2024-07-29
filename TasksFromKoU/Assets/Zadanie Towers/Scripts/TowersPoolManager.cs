using UnityEngine;
using UnityEngine.Pool;

public class TowersPoolManager : MonoBehaviour
{
    [SerializeField]
    private Tower _towerPrefab;
    [SerializeField]
    private int _defaultCapacity = 100;
    [SerializeField]
    private int _maxCapacity = 300;

    private ObjectPool<Tower> _towerPool;

    public ObjectPool<Tower> TowerPool => _towerPool;
    public static TowersPoolManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _towerPool = new ObjectPool<Tower>(CreateTower, (Tower tower) =>
        {
            tower.gameObject.SetActive(true);
        }, (Tower tower) =>
        {
            tower.gameObject.SetActive(false);
        }, (Tower tower) =>
        {
            Destroy(tower.gameObject);
        }, false, _defaultCapacity, _maxCapacity);
    }

    private Tower CreateTower()
    {
        Tower tower = Instantiate(_towerPrefab);
        tower.gameObject.transform.SetParent(transform);
        return tower;
    }

    public Tower GetTower()
    {
        Tower tower = _towerPool.Get();
        return tower;
    }

    public void ReturnTowerBackToPool(Tower tower)
    {
        _towerPool.Release(tower);
        tower.gameObject.transform.SetParent(transform);
    }
}
