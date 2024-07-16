using UnityEngine;

public class GravityBallForceController : MonoBehaviour
{
    [SerializeField]
    private GravityBallSpawner _ballSpawner;
    [SerializeField]
    private float _g;

    private void FixedUpdate()
    {
        for (int i = 0; i < _ballSpawner.GravityBalls.Count; i++)
        {
            for (int j = i + 1; j < _ballSpawner.GravityBalls.Count; j++)
            {
                if (i == j || !_ballSpawner.GravityBalls[i].IsActive || !_ballSpawner.GravityBalls[j].IsActive)
                {
                    continue;
                }
                float g = _ballSpawner.ForceType == ForceType.Attract ? _g : -_g;
                Vector2 direction = (_ballSpawner.GravityBalls[j].transform.position - _ballSpawner.GravityBalls[i].transform.position).normalized;
                float distanceBetweenBalls = Vector2.Distance(_ballSpawner.GravityBalls[i].transform.position, _ballSpawner.GravityBalls[j].transform.position);
                float forcePower = distanceBetweenBalls > Mathf.Epsilon ? CalculateGravitationalForces(_ballSpawner.GravityBalls[i].BallRigidbody.mass, _ballSpawner.GravityBalls[j].BallRigidbody.mass, g,
                    distanceBetweenBalls) : 1;

                Vector2 force = direction * forcePower;
                _ballSpawner.GravityBalls[i].BallRigidbody.AddForce(force);
                _ballSpawner.GravityBalls[j].BallRigidbody.AddForce(-1 * force);
            }
        }
    }

    private float CalculateGravitationalForces(float firstBallMass, float secondBallMass, float G, float centerToCenter)
    {
        return (G * firstBallMass * secondBallMass) / (centerToCenter * centerToCenter);
    }
}
