using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovement : MonoBehaviour
{
    private float _amplitude = 0.02f;
    private float _speed = 3f;

    private Vector3 _startPos;
    private float _randomOffset;

    void Start()
    {
        _startPos = transform.localPosition;
        _randomOffset = Random.Range(5f, 10f);
    }

    void Update()
    {
        if (GlobalVariables.teammateCaught)
        {
            float offset = Mathf.Sin((Time.time + _randomOffset) * 2*_speed) * _amplitude*6;
            transform.localPosition = _startPos + new Vector3(0, offset, 0);
        }
        else
        {
            float offset = Mathf.Sin((Time.time + _randomOffset) * _speed) * _amplitude;
            transform.localPosition = _startPos + new Vector3(0, offset, 0);
        } 
    }
}
