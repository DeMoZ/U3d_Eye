using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTest : MonoBehaviour
{
    [Range(-180, 180)]
    [SerializeField] float angle = 0;

    [SerializeField] Transform _ball;
    [SerializeField] Vector2 _direction = Vector2.zero;
    Transform _transform;
    Vector3 lookPoint = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {

        lookPoint = Quaternion.Euler(_direction.normalized * angle) * _transform.forward + _transform.position;
    }

    private void OnDrawGizmos()
    {

        // forward 
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (transform.position + transform.forward));

        // lookPoint
        Vector3 point = (lookPoint == Vector3.zero) ? transform.position + transform.forward : lookPoint;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, point);
        Gizmos.DrawSphere(point, 0.1f);
    }
}
