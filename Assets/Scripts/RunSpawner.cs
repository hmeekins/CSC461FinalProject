using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public GameObject enemyPrefab;
    public float enemyFollowStartDelay = 0.2f;
    public float spawnHeight;
    public float speed;
    public float offsetX;
    public float offsetZ;

    
    private int variation;
    private GameObject rightRunner;

    private GameObject leftRunner;
    private BoxCollider box;
    private AtTarget rightRunnerTarget;
    private AtTarget leftRunnerTarget;
    private bool runActive;
    void Start()
    {
        box = GetComponent<BoxCollider>();
    }
    
    void Update()
    {
        if (rightRunner == null && leftRunner == null) 
        {
            SpawnFootballPlayer();
            runActive = false;
        }
        if (GlobalVariables.isHolding)
        {
            runActive = true;
        }
        if (runActive)
        {
            Run();
        }
    }

    private void SpawnFootballPlayer()
    {
        var bounds = box.bounds;

        Vector3 leftSpawnPosition = new Vector3(bounds.max.x - offsetX, spawnHeight, bounds.max.z - offsetZ);
        leftRunner = Instantiate(
            objectPrefab,
            leftSpawnPosition,
            Quaternion.identity
        );

        leftRunnerTarget = leftRunner.GetComponent<AtTarget>();
        leftRunnerTarget.playerPosition = "Left";
        SpawnOpponentForTarget(leftRunner.transform, leftSpawnPosition);

        Vector3 rightSpawnPosition = new Vector3(bounds.min.x + offsetX, spawnHeight, bounds.max.z - offsetZ);
        rightRunner = Instantiate(
            objectPrefab,
            rightSpawnPosition,
            Quaternion.identity
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

        if (!rightRunnerTarget.atTargetZ)
        {
            pos = rightRunner.transform.position;
            targetPosition = new Vector3(pos.x, pos.y, GlobalVariables.rightTargetZ);
            rightRunner.transform.position = Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }
        
        if (!leftRunnerTarget.atTargetZ)
        {
            pos = leftRunner.transform.position;
            targetPosition = new Vector3(pos.x, pos.y, GlobalVariables.leftTargetZ);
            leftRunner.transform.position = Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }

        if (!rightRunnerTarget.atTargetX && rightRunnerTarget.atTargetZ)
        {
            pos = rightRunner.transform.position;
            targetPosition = new Vector3(GlobalVariables.rightTargetX, pos.y, pos.z);
            rightRunner.transform.position = Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }

        if (!leftRunnerTarget.atTargetX && leftRunnerTarget.atTargetZ)
        {
            pos = leftRunner.transform.position;
            targetPosition = new Vector3(GlobalVariables.leftTargetX, pos.y, pos.z);
            leftRunner.transform.position = Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }
    }
}
