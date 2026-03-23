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
    private RusherCollision rusherCollision;
    private GameObject player;

    void Start()
    {
        timer = 0f;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (GameFlowController.Instance.Variation == GameVariation.Variation1 ||
        GameFlowController.Instance.Variation == GameVariation.Variation4)
        {
            startSpeed = 0;
            maxSpeed = 0;
        }
        else if (GameFlowController.Instance.Variation == GameVariation.Variation2 ||
        GameFlowController.Instance.Variation == GameVariation.Variation5)
        {
            startSpeed = 3;
            maxSpeed = 9;
        }
        else
        {
            startSpeed = 5;
            maxSpeed = 11;
        }
    }

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.PlayRunning && !GlobalVariables.tackled)
        {
            Run();
        }
    }

    void Run()
    {
        if (!GlobalVariables.ballThrown) {
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
}
