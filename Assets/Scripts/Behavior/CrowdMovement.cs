using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovement : MonoBehaviour
{
    private float amplitude = 0.02f;
    private float speed = 3f;

    Vector3 startPos;
    float randomOffset;

    void Start()
    {
        startPos = transform.localPosition;
        randomOffset = Random.Range(5f, 10f);
    }

    void Update()
    {
        if (GlobalVariables.teammateCaught)
        {
            float offset = Mathf.Sin((Time.time + randomOffset) * 2*speed) * amplitude*6;
            transform.localPosition = startPos + new Vector3(0, offset, 0);
        }
        else
        {
            float offset = Mathf.Sin((Time.time + randomOffset) * speed) * amplitude;
            transform.localPosition = startPos + new Vector3(0, offset, 0);
        } 
    }
}
