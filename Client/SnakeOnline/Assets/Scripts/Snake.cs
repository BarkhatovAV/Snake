using System;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Tail _tailPrefab;
    [SerializeField] private Transform _head;
    //[SerializeField] private Transform _directionPoint;
    [SerializeField] private float _speed = 2;
    //[SerializeField] private float _rotateSpeed = 90;

    private Tail _tail;

    //private Vector3 _targetDirection = Vector3.zero;

    public float Speed {  get { return _speed; } }

    private void Update()
    {
        //Rotate();
        Move();
    }

    public void Init(int detailCount)
    {
       
        _tail = Instantiate(_tailPrefab, transform.position, Quaternion.identity);
        _tail.Init(_head, _speed, detailCount);
    }

    public void Destroy()
    {
        Destroy(gameObject);
        _tail.Destroy();
    }

    //private void Rotate()
    //{
    //    if (_targetDirection == Vector3.zero)
    //        return;
    //    Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
    //    _head.rotation = Quaternion.RotateTowards(_head.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
    //    //float diffY = _directionPoint.eulerAngles.y - _head.eulerAngles.y;

    //    //if (diffY > 180)
    //    //    diffY = (diffY - 180) * -1;
    //    //else if (diffY < -180)
    //    //    diffY = (diffY + 180) * -1;

    //    //float maxAngle = Time.deltaTime * _rotateSpeed;
    //    //float rotateY = Mathf.Clamp(diffY, -maxAngle, maxAngle);

    //    //_head.Rotate(0, rotateY, 0);
    //}

    public void SetDetailCount(int detailCount)
    {
        _tail.SetDetailCount(detailCount);
    }

    private void Move()
    {
        transform.position += _head.forward * Time.deltaTime * _speed;
    }

    public void SetRotation(Vector3 pointToLook)
    {
        _head.LookAt(pointToLook);
    }

    //public void LerpRotation(Vector3 cursorPosition)
    //{
    //    _targetDirection = cursorPosition - _head.position;
    //    //_directionPoint.LookAt(cursorPosition);
    //}

    //public void GetMoveInfo(out Vector3 position)
    //{
    //    position = transform.position;
    //}
}
