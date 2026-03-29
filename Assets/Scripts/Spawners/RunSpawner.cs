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
    private BoxCollider box;
    private TeammateMovement rightRunnerPosition;
    private TeammateMovement leftRunnerPosition;

    void Start()
    {
        box = GetComponent<BoxCollider>();
    }
    
    void Update()
    {
        if (GameFlowController.Instance.State == GameState.WaitingForSnap && rightRunner == null && leftRunner == null) 
            SpawnFootballPlayer();
    }

    /// <summary>
    /// Spawns teammates and finds the target points they need to run to
    /// </summary>
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

        leftRunnerPosition = leftRunner.GetComponent<TeammateMovement>();
        leftRunnerPosition.playerPosition = "Left";
        SpawnOpponentForTeammate(leftRunner.transform, leftSpawnPosition);

        Vector3 rightSpawnPosition = new Vector3(bounds.min.x, spawnHeight, bounds.max.z);
        rightRunner = Instantiate(
            objectPrefab,
            rightSpawnPosition,
            spawnRotation
        );

        rightRunnerPosition = rightRunner.GetComponent<TeammateMovement>();
        rightRunnerPosition.playerPosition = "Right";
        SpawnOpponentForTeammate(rightRunner.transform, rightSpawnPosition);

        GetTargets();
    }

    /// <summary>
    /// Spawns opponent to follow teammate
    /// </summary>
    /// <param name="target">Object for opponent to follow</param>
    /// <param name="spawnPos">Spawn location of opponent</param>
    private void SpawnOpponentForTeammate(Transform target, Vector3 spawnPos)
    {
        Vector3 enemyPos = spawnPos + new Vector3(0f, 0f, -5f);
        GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);

        EnemyFollower follower = enemy.GetComponent<EnemyFollower>();
        follower.target = target;
    }

    /// <summary>
    /// Gets target points for players to run to. Two variations (Left runner goes far, right runner goes short and vice versa)
    /// </summary>
    private void GetTargets()
    {
        var bounds = box.bounds;
        int variation = GetPlayVariation();
        float lengthOfField = bounds.max.z - bounds.min.z;
        float startZ = bounds.max.z - 0.2f * lengthOfField;
        float midZ   = bounds.min.z + 0.5f * lengthOfField;

        if (variation == 0)
        {
            GlobalVariables.leftTargetZ  = UnityEngine.Random.Range(midZ, startZ);
            GlobalVariables.rightTargetZ = UnityEngine.Random.Range(bounds.min.z, midZ);
        }
        else if (variation == 1)
        {
            GlobalVariables.rightTargetZ  = UnityEngine.Random.Range(midZ, startZ);
            GlobalVariables.leftTargetZ = UnityEngine.Random.Range(bounds.min.z, midZ);
        }
        else
        {
            GlobalVariables.leftTargetZ = UnityEngine.Random.Range(bounds.min.z, midZ);
            GlobalVariables.rightTargetZ = UnityEngine.Random.Range(bounds.min.z, midZ);
        }

        GlobalVariables.leftTargetX = bounds.min.x;
        GlobalVariables.rightTargetX = bounds.max.x;
    }

    /// <summary>
    /// Gets play variation based on number of successful passes the player makes
    /// </summary>
    /// <returns>Play Variation</returns>
    private int GetPlayVariation()
    {
        int variation;
        if (GlobalVariables.successfulPasses < 4 || !GameFlowController.Instance.DifficultyScaling)
            variation = UnityEngine.Random.Range(0, 2);
        else if (GlobalVariables.successfulPasses < 8)
            variation = UnityEngine.Random.Range(0, 3);
        else
            variation = 2;
        return variation;
    }
}
