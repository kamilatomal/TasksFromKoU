using UnityEngine;

public class BallForceController : MonoBehaviour
{
    [SerializeField]
    private BallSpawner _ballSpawner;
    [SerializeField]
    private float _g;

    public float G => _g;

    private void FixedUpdate()
    {
        for (int i = 0; i < _ballSpawner.GravityBalls.Count; i++)
        {
            for (int j = 0; j < _ballSpawner.GravityBalls.Count; j++)
            {
                if (i == j || !_ballSpawner.GravityBalls[i].BallCollider.isActiveAndEnabled || !_ballSpawner.GravityBalls[j].BallCollider.isActiveAndEnabled)
                {
                    continue;
                }
                float g = _ballSpawner.ForceType == ForceType.Attract ? _g : -_g;
                Vector2 direction = (_ballSpawner.GravityBalls[j].transform.position - _ballSpawner.GravityBalls[i].transform.position).normalized;
                float force = CalculateGravitationalForces(_ballSpawner.GravityBalls[i].BallRigidbody.mass, _ballSpawner.GravityBalls[j].BallRigidbody.mass, g, 
                    Vector2.Distance(_ballSpawner.GravityBalls[i].transform.position, _ballSpawner.GravityBalls[j].transform.position));
                _ballSpawner.GravityBalls[i].BallRigidbody.AddForce(direction * force);
            }
        }
    }

    private float CalculateGravitationalForces(float firstBallMass, float secondBallMass, float G, float centerToCenter)
    {
        return (G * firstBallMass * secondBallMass) / (centerToCenter * centerToCenter);
    }
}
