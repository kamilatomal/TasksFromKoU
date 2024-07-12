using UnityEngine;

public class AttractController : MonoBehaviour
{
    [SerializeField]
    private BallSpawner _ballSpawner;
    [SerializeField]
    private float _g;

    private void FixedUpdate()
    {
        for (int i = 0; i < _ballSpawner.GravityBalls.Count; i++)
        {
            for (int j = 0; j < _ballSpawner.GravityBalls.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                Vector2 direction = (_ballSpawner.GravityBalls[j].transform.position - _ballSpawner.GravityBalls[i].transform.position).normalized;
                float force = CalculateGravitationalForces(_ballSpawner.GravityBalls[i].BallRigidbody.mass, _ballSpawner.GravityBalls[j].BallRigidbody.mass, _g, 
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
