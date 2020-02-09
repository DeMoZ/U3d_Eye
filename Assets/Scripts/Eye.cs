#define DRAW_MARKERS
using System.Collections;
using UnityEngine;

public class Eye : MonoBehaviour
{
#if DRAW_MARKERS
    public bool _DrawMarkers = true;
#endif
    [Tooltip("Moving part or the EYE")]
    [SerializeField] Transform _ball;
    [Tooltip("Limit of the Ball movement in every direction")]
    [Range(0, 180)]
    [SerializeField] float _movingAngle = 30f;
    [Tooltip("Every random second from that range look direction will change")]
    [SerializeField] Vector2 _directionChangeRamdomRange = new Vector2(1f, 4f);
    [Tooltip("Random range for eye movement speed")]
    [SerializeField] Vector2 _movingSpeedRandomRange = new Vector2(10f, 100f);
    [Tooltip("Target to look at")]
    [SerializeField] Transform _target;
    [Tooltip("Spot distance")]
    [SerializeField] float _spotDistance = 4f;

    float lookingTimer = 0;

    Vector3 lookPoint = Vector3.zero;
    Vector3 randomVector = Vector3.zero;
    float randomAngle = 0;
    float randomSpeed = 0;

    Transform _transform;

    public enum EyeState
    {
        Random,
        Focuse,
        Sleep
    }
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;

        _directionChangeRamdomRange = Sort(_directionChangeRamdomRange);
        _movingSpeedRandomRange = Sort(_movingSpeedRandomRange);
    }

    // Update is called once per frame
    void Update()
    {
        lookingTimer -= Time.deltaTime;

        if (lookingTimer <= 0)
        {
            lookingTimer = Random.Range(_directionChangeRamdomRange.x, _directionChangeRamdomRange.y);
            randomAngle = Random.Range(-1 * _movingAngle, _movingAngle);
            randomVector = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            randomSpeed = Random.Range(_movingSpeedRandomRange.x, _movingSpeedRandomRange.y);
        }

        if (_target == null || Vector3.Distance(_target.position, _transform.position) > _spotDistance)
        {
            //lookPoint = LookAtRandomPoint();
            lookPoint = Quaternion.Euler(randomVector * randomAngle) * _transform.forward + _transform.position;
        }
        else
        {
            lookPoint = _target.position;
        }

        //_ball.LookAt(lookPoint, _transform.up);
               
        Quaternion toRotation = Quaternion.LookRotation(lookPoint - transform.position, _transform.up);
        _ball.rotation = Quaternion.RotateTowards(_ball.rotation, toRotation, randomSpeed * Time.deltaTime);
    }

    private Vector3 LookAtRandomPoint()
    {
        lookingTimer -= Time.deltaTime;

        if (lookingTimer <= 0)
        {
            lookingTimer = Random.Range(_directionChangeRamdomRange.x, _directionChangeRamdomRange.y);
            randomAngle = Random.Range(-1 * _movingAngle, _movingAngle);
            randomVector = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        }

        return Quaternion.Euler(randomVector * randomAngle) * _transform.forward + _transform.position;
    }

    private Vector3 LookAtTarget()
    {
        return Vector3.zero;
    }

    /// <summary>
    /// Sort Vector2
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private Vector2 Sort(Vector2 vector)
    {
        return Sort(vector.x, vector.y);
    }

    /// <summary>
    /// Sort Vector2
    /// </summary>
    /// <returns></returns>
    private Vector2 Sort(float x, float y)
    {
        if (x > y)
        {
            float a = x;
            x = y;
            y = a;
        }
        return new Vector2(x, y);
    }

#if DRAW_MARKERS
    private void OnDrawGizmos()
    {
        if (!_DrawMarkers) return;
        //if (_transform == null)
        // _transform = transform;

        // forward 
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (transform.position + transform.forward));

        // lookPoint
        Vector3 point = (lookPoint == Vector3.zero) ? transform.position + transform.forward : lookPoint;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, point);
        Gizmos.DrawSphere(lookPoint, 0.1f);
    }
#endif
}
