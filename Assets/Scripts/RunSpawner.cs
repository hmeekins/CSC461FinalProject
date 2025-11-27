using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpawner : MonoBehaviour
{
    public GameObject objectPrefab;

    public float spawnHeight;
    public float speed;
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
        Vector3 leftSpawnPosition = new Vector3(bounds.max.x, spawnHeight, bounds.max.z);
        leftRunner = Instantiate(
            objectPrefab,
            leftSpawnPosition,
            Quaternion.identity
        );
        leftRunnerTarget = leftRunner.GetComponent<AtTarget>();
        leftRunnerTarget.playerPosition = "Left";

        Vector3 rightSpawnPosition = new Vector3(bounds.min.x, spawnHeight, bounds.max.z);
        rightRunner = Instantiate(
            objectPrefab,
            rightSpawnPosition,
            Quaternion.identity
        );
        rightRunnerTarget = rightRunner.GetComponent<AtTarget>();
        rightRunnerTarget.playerPosition = "Right";

        GetTargets();

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
