using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherSpawner : MonoBehaviour
{
    public Transform target;

    public float startSpeed;

    public float maxSpeed;

    public float speedUpTime;

    public float currentSpeed;
    private float timer;

    private bool runActive;

    void Start()
    {
        currentSpeed = startSpeed;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        //Speed up rusher over time
        float t = Mathf.Clamp01(timer / speedUpTime);
        currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);

        Vector3 current = transform.position;

        Vector3 targetPos = new Vector3(
            target.position.x,
            current.y,
            target.position.z
        );

        Vector3 direction = (targetPos - current).normalized;
        transform.position += direction * currentSpeed * Time.deltaTime;
    }


}
