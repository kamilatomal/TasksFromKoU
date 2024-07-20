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
        _towerSpawner.OnBallSpawned += UpdateBallAmountText;
        _towerSpawner.OnBallDestroyedAction += UpdateBallAmountText;
    }

    private void OnDisable()
    {
        _towerSpawner.OnBallSpawned -= UpdateBallAmountText;
        _towerSpawner.OnBallDestroyedAction -= UpdateBallAmountText;
    }

    private void UpdateBallAmountText()
    {
        _towerAmountText.text = _towerSpawner.TowerBalls.Count.ToString();
    }
}
