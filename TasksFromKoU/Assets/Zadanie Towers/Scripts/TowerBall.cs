using UnityEngine;

public class TowerBall : MonoBehaviour
{
    [SerializeField]
    private float _minMovementUnit;
    [SerializeField]
    private float _maxMovementUnit;

    private void Start()
    {
        MoveBall();
    }
    private void MoveBall()
    {
        var randomMovementUnit = Random.Range(_minMovementUnit, _maxMovementUnit);
        transform.position += new Vector3(randomMovementUnit, randomMovementUnit, 0);
    }
}
