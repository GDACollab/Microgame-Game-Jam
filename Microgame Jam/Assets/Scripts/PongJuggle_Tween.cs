using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PongJuggle_Tween : MonoBehaviour
{
    [SerializeField] private float _speedMetersPerSecond = 25f;

    [SerializeField] private Vector3 _destination;
    private Vector3 _startPosition;
    private float _totalLerpDuration;
    private float _elapsedLerpDuration;
    private Action _onCompleteCallback;

    private void Start()
    {
        var distanceToNextWaypoint = Vector3.Distance(a: transform.position, b: _destination);
        _totalLerpDuration = distanceToNextWaypoint / _speedMetersPerSecond;

        _startPosition = transform.position;
        _elapsedLerpDuration = 0f;
        //_onCompleteCallback = onComplete;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (_destination.HasValue == false)
            return;
        */
        
        if(_elapsedLerpDuration >= _totalLerpDuration && _totalLerpDuration > 0)
            return;
        
        _elapsedLerpDuration += Time.deltaTime;
        float percent = (_elapsedLerpDuration / _totalLerpDuration);

        //transform.position = Vector3.Lerp(a: _startPosition, b: _destination.Value, percent);
        transform.position = Vector3.Lerp(a: _startPosition, b: _destination, percent);

        if (_elapsedLerpDuration >= _totalLerpDuration)
            _onCompleteCallback?.Invoke();
        
    }

    /*
    public void MoveTo(Vector3 destination, Action onComplete = null)
    {
        var distanceToNextWaypoint = Vector3.Distance(a: transform.position, b: destination);
        _totalLerpDuration = distanceToNextWaypoint / _speedMetersPerSecond;

        _startPosition = transform.position;
        _destination = destination;
        _elapsedLerpDuration = 0f;
        _onCompleteCallback = onComplete;
    }
    */
}
