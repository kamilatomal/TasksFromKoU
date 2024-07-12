using TMPro;
using UnityEngine;

public class BallSpawnerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _ballAmountText;
    [SerializeField]
    private BallSpawner _ballSpawner;

    private string _amountText = "Balls amount: ";

    private void Start()
    {
        UpdateBallAmountText();
    }

    private void OnEnable()
    {
        _ballSpawner.OnBallSpawned += UpdateBallAmountText;
    }

    private void OnDisable()
    {
        _ballSpawner.OnBallSpawned -= UpdateBallAmountText;
    }

    private void UpdateBallAmountText()
    {
        _ballAmountText.text = _amountText + _ballSpawner.SpawnedBallAmount.ToString();
    }
}
