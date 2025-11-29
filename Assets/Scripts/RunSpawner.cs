using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public GameObject enemyPrefab;
    public float spawnHeight;
    public float speed;
    public GameObject rightRunner;

    public GameObject leftRunner;
    private int variation;
    private BoxCollider box;
    private AtTarget rightRunnerTarget;
    private AtTarget leftRunnerTarget;

    void Start()
    {
        box = GetComponent<BoxCollider>();
    }
    
    void Update()
    {
        if (rightRunner == null && leftRunner == null) 
        {
            SpawnFootballPlayer();
            GlobalVariables.runActive = false;
        }
        if (GlobalVariables.isHolding)
        {
            GlobalVariables.runActive = true;
        }
        if (GlobalVariables.runActive)
        {
            Run();
        }
    }

    private void SpawnFootballPlayer()
    {
        var bounds = box.bounds;

        Quaternion spawnRotation = Quaternion.LookRotation(Vector3.back);
        Vector3 leftSpawnPosition = new Vector3(bounds.max.x, spawnHeight, bounds.max.z);
        leftRunner = Instantiate(
            objectPrefab,
            leftSpawnPosition,
            spawnRotation
        );

        leftRunnerTarget = leftRunner.GetComponent<AtTarget>();
        leftRunnerTarget.playerPosition = "Left";
        SpawnOpponentForTarget(leftRunner.transform, leftSpawnPosition);

        Vector3 rightSpawnPosition = new Vector3(bounds.min.x, spawnHeight, bounds.max.z);
        rightRunner = Instantiate(
            objectPrefab,
            rightSpawnPosition,
            spawnRotation
        );

        rightRunnerTarget = rightRunner.GetComponent<AtTarget>();
        rightRunnerTarget.playerPosition = "Right";
        SpawnOpponentForTarget(rightRunner.transform, rightSpawnPosition);

        GetTargets();
    }

    private void SpawnOpponentForTarget(Transform target, Vector3 spawnPos)
    {
        Vector3 enemyPos = spawnPos + new Vector3(0f, 0f, -2f);
        GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);

        EnemyFollower follower = enemy.GetComponent<EnemyFollower>();
        follower.target = target;
    }

    private void GetTargets()
    {
        var bounds = box.bounds;
        variation = UnityEngine.Random.Range(0, 2);
        float lengthOfField = bounds.max.z - bounds.min.z;

        float startZ = bounds.max.z - 0.2f * lengthOfField;
        float midZ   = bounds.min.z + 0.5f * lengthOfField;

        if (variation == 0)
        {
            GlobalVariables.leftTargetZ  = UnityEngine.Random.Range(midZ, startZ);
            GlobalVariables.rightTargetZ = UnityEngine.Random.Range(bounds.min.z, midZ);
        }
        if (variation == 1)
        {
            GlobalVariables.rightTargetZ  = UnityEngine.Random.Range(midZ, startZ);
            GlobalVariables.leftTargetZ = UnityEngine.Random.Range(bounds.min.z, midZ);
        }

        GlobalVariables.leftTargetX = bounds.min.x;
        GlobalVariables.rightTargetX = bounds.max.x;
    }

    private void Run()
    {
        Vector3 pos;
        Vector3 targetPosition;
        var bounds = box.bounds;

        if (!rightRunnerTarget.atTargetZ && rightRunner != null)
        {
            pos = rightRunner.transform.position;
            targetPosition = new Vector3(pos.x, pos.y, GlobalVariables.rightTargetZ);

            rightRunner.transform.position =
            Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }

        if (!leftRunnerTarget.atTargetZ && leftRunner != null)
        {
            pos = leftRunner.transform.position;
            targetPosition = new Vector3(pos.x, pos.y, GlobalVariables.leftTargetZ);

            leftRunner.transform.position =
            Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }

        if (!rightRunnerTarget.atTargetX && rightRunnerTarget.atTargetZ && rightRunner != null)
        {
            pos = rightRunner.transform.position;
            targetPosition = new Vector3(GlobalVariables.rightTargetX, pos.y, pos.z);

            Vector3 moveDir = targetPosition - pos;
            if (moveDir != Vector3.zero)
                rightRunner.transform.rotation = Quaternion.LookRotation(moveDir);

            rightRunner.transform.position =
                Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }

        if (!leftRunnerTarget.atTargetX && leftRunnerTarget.atTargetZ && leftRunner != null)
        {
            pos = leftRunner.transform.position;
            targetPosition = new Vector3(GlobalVariables.leftTargetX, pos.y, pos.z);

            Vector3 moveDir = targetPosition - pos;
            if (moveDir != Vector3.zero)
                leftRunner.transform.rotation = Quaternion.LookRotation(moveDir);

            leftRunner.transform.position =
                Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }
    }
}
