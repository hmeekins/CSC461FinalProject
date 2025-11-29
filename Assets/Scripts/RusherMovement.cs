using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherMovement : MonoBehaviour
{
    public Transform target;
    public float startSpeed;
    public float maxSpeed;
    public float speedUpTime;
    
    private float currentSpeed;
    private float timer;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if (GlobalVariables.runActive)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / speedUpTime);
            currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);

            Vector3 current = gameObject.transform.position;

            Vector3 targetPos = new Vector3(
                target.position.x,
                current.y,
                target.position.z
            );

            Vector3 direction = (targetPos - current).normalized;
            gameObject.transform.position += direction * currentSpeed * Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GlobalVariables.runEnd = true;
            GlobalVariables.runActive = false;
            GlobalVariables.lives -= 1;
            GameObject ball = GameObject.FindWithTag("Ball");
            Destroy(ball);
            Destroy(gameObject);
        }
    }
}
