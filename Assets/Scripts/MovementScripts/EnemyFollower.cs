using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyFollower : MonoBehaviour
{
    public Transform target;

    public float startSpeed;     
    public float minSpeed;       
    public float slowDownTime;
    public float stopDistance;
    public float rotationSpeed;

    private float currentSpeed;
    private float timer;
    private bool runStart = true;

    void Start()
    {
        currentSpeed = startSpeed;
        timer = 0f;
    }

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.PlayRunning)
        {
            if (runStart)
                StartCoroutine(waitToMove());
            else
                Run();
        }
    }

     private IEnumerator waitToMove()
    {
        yield return new WaitForSeconds(1.2f);
        runStart = false;
    }

    /// <summary>
    /// Runs toward target, slows down as time increases.
    /// </summary>
    void Run()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / slowDownTime);
        currentSpeed = Mathf.Lerp(startSpeed, minSpeed, t);

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 current = transform.position;

        Vector3 targetPos = new Vector3(
            target.position.x,
            current.y,
            target.position.z
        );

        float dist = Vector3.Distance(current, targetPos);

        if (dist <= stopDistance)
            return;

        Vector3 direction = (targetPos - current).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        transform.position += direction * currentSpeed * Time.deltaTime;
    }
}
