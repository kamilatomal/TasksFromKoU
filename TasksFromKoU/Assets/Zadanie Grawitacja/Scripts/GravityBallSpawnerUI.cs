using TMPro;
using UnityEngine;

public class GravityBallSpawnerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _ballAmountText;
    [SerializeField]
    private GravityBallSpawner _ballSpawner;

    private void Start()
    {
        UpdateBallAmountText();
    }

    private void OnEnable()
    {
        _ballSpawner.OnBallSpawned += UpdateBallAmountText;
        _ballSpawner.OnBallDestroyedAction += UpdateBallAmountText;
    }

    private void OnDisable()
    {
        _ballSpawner.OnBallSpawned -= UpdateBallAmountText;
        _ballSpawner.OnBallDestroyedAction -= UpdateBallAmountText;
    }

    private void UpdateBallAmountText()
    {
        _ballAmountText.text = _ballSpawner.GravityBalls.Count.ToString();
    }
}
