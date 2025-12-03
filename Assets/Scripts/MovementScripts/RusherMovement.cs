using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherMovement : MonoBehaviour
{
    public Transform target;
    public float startSpeed;
    public float maxSpeed;
    public float speedUpTime;
    public float rotationSpeed;

    private float currentSpeed;
    private float timer;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.PlayRunning)
            Run();

        if (GlobalVariables.ballThrown)
        {
            SphereCollider collider = GetComponent<SphereCollider>();
            collider.enabled = false;
        }
    }

    void Run()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / speedUpTime);
        currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);

        Vector3 current = transform.position;

        Vector3 targetPos = new Vector3(
            target.position.x,
            current.y,
            target.position.z
        );

        Vector3 direction = (targetPos - current).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        transform.position += direction * currentSpeed * Time.deltaTime;
    }
}
