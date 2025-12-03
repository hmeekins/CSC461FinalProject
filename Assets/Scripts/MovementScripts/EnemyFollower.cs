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

    void Start()
    {
        currentSpeed = startSpeed;
        timer = 0f;
    }

    void Update()
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
