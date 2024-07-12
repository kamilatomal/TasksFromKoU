using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private SlingshotController _ballSpawner;
    [SerializeField]
    private CinemachineCamera _cinemachineCamera;
    [SerializeField]
    private CinemachinePositionComposer _cinemachinePositionComposer;
    [SerializeField]
    private float _ballOrthographicSize = 10f;

    private Transform _defaultTrack;
    private Vector3 _ballOffset;
    private Vector3 _slingOffset;
    private float _slingOrthographicSize;

    private void Start()
    {
        _defaultTrack = _cinemachineCamera.Follow;
        _ballOffset = new Vector3(0, 0, 0);
        _slingOffset = _cinemachinePositionComposer.TargetOffset;
        _slingOrthographicSize = _cinemachineCamera.Lens.OrthographicSize;
    }

    private void OnEnable()
    {
        _ballSpawner.OnActiveBallLandedAction += TrackSling;
        _ballSpawner.OnActiveBallShootAction += TrackBall;
    }

    private void OnDisable()
    {
        _ballSpawner.OnActiveBallLandedAction -= TrackSling;
        _ballSpawner.OnActiveBallShootAction -= TrackBall;
    }

    private void TrackBall(Ball ball)
    {
        _cinemachineCamera.Follow = ball.transform;
        _cinemachinePositionComposer.TargetOffset = _ballOffset;
        _cinemachineCamera.Lens.OrthographicSize = _ballOrthographicSize;
    }

    private void TrackSling(Ball _)
    {
        TrackSling();
    }

    private void TrackSling()
    {
        _cinemachineCamera.Follow = _defaultTrack;
        _cinemachinePositionComposer.TargetOffset = _slingOffset;
        _cinemachineCamera.Lens.OrthographicSize = _slingOrthographicSize;
    }
}
