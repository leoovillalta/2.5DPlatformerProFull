using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private int _initialFloor = 2;
    private int _floorRequest;
    [SerializeField]
    private float _movingSpeed = 3.0f;
    [SerializeField]
    private float _timeDelayPerFloor = 5.0f;
    [SerializeField]
    private Transform _firstFloor, _secondFloor;
    private Transform _floorPositionToMove;
    private bool _moving = false;
    private bool _moveElevator = false;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        defineFloorToHeadTo(_initialFloor);
        StartCoroutine(WaitInFloor());
    }
    void defineFloorToHeadTo(int _actualFloor)
    {
       
        if (_actualFloor == 1)
        {
            _floorPositionToMove = _secondFloor;
            _floorRequest = 2;
        }
        else if (_actualFloor == 2)
        {
            _floorPositionToMove = _firstFloor;
            _floorRequest = 1;
        }
        Debug.Log("defineFloorToHeadTo() floor request= " + _floorRequest);
    }
    IEnumerator WaitInFloor()
    {
        Debug.Log("WaitingInFloor()");
        yield return new WaitForSeconds(_timeDelayPerFloor);
        _moveElevator = true;
        _moving = true;
    }
    void MoveElevator()
    {
        transform.position = Vector3.MoveTowards(transform.position, _floorPositionToMove.position, _movingSpeed * Time.deltaTime);

    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_moving && (transform.position.y ==_floorPositionToMove.position.y))
        {
            _moveElevator = false;
            count++;
            Debug.Log("Times Moved: " + count);
            _moving = false;
            defineFloorToHeadTo(_floorRequest);
            StartCoroutine(WaitInFloor());
        }
        if (_moveElevator)
        {
            MoveElevator();
        }
    }
}
