using TMPro;
using UnityEngine;

public class TowerSpawnerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _towerAmountText;
    [SerializeField]
    private TowerSpawner _towerSpawner;

    private void Start()
    {
        UpdateBallAmountText();
    }

    private void OnEnable()
    {
        _towerSpawner.OnTowerSpawned += UpdateBallAmountText;
        _towerSpawner.OnTowerDestroyedAction += UpdateBallAmountText;
    }

    private void OnDisable()
    {
        _towerSpawner.OnTowerSpawned -= UpdateBallAmountText;
        _towerSpawner.OnTowerDestroyedAction -= UpdateBallAmountText;
    }

    private void UpdateBallAmountText()
    {
        _towerAmountText.text = _towerSpawner.Towers.Count.ToString();
    }
}
