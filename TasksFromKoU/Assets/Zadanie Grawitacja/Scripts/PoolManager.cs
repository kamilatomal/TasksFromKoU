using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private GravityBall _gravityBallPrefab;
    [SerializeField]
    private int _defaultCapacity = 100;
    [SerializeField]
    private int _maxCapacity = 300;

    private ObjectPool<GravityBall> _gravityBallPool;

    public ObjectPool<GravityBall> GravityBallPool => _gravityBallPool;
    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
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
        _gravityBallPool = new ObjectPool<GravityBall>(CreateBall, (GravityBall gravityBall) =>
        {
            gravityBall.gameObject.SetActive(true);
        }, (GravityBall gravityBall) =>
        {
            gravityBall.gameObject.SetActive(false);
            gravityBall.ResetBall(true);
        }, (GravityBall gravityBall) =>
        {
            Destroy(gravityBall.gameObject);
        }, false, _defaultCapacity, _maxCapacity);
    }

    private GravityBall CreateBall()
    {
        GravityBall gravityBall = Instantiate(_gravityBallPrefab);
        gravityBall.gameObject.transform.SetParent(transform);
        return Instantiate(gravityBall);
    }

    public GravityBall GetBall()
    {
        GravityBall gravityBall = _gravityBallPool.Get();
        return gravityBall;
    }

    public void ReturnBallBackToPool(GravityBall gravityBall)
    {
        _gravityBallPool.Release(gravityBall);
        gravityBall.gameObject.transform.SetParent(transform);
    }
}
