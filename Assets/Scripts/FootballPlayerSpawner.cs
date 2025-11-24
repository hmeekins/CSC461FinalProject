using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballPlayerSpawner : MonoBehaviour
{
    public GameObject objectPrefab;

    public float spawnHeight;
    public float distanceMultiplier;
    public int difficultyIncreaseThresh;
    private GameObject currentObject;
    private BoxCollider box;
    private int previousMilestone;

    void Start()
    {
        box = GetComponent<BoxCollider>();
        previousMilestone = 0;
    }
    void Update()
    {
        if (currentObject == null) 
        {
            SpawnFootballPlayer();
        }
        if (GlobalVariables.score - previousMilestone == difficultyIncreaseThresh && distanceMultiplier <= 1)
        {
            distanceMultiplier += .1f;
            previousMilestone = GlobalVariables.score;
            Debug.Log(GlobalVariables.score);
        }
    }

    private void SpawnFootballPlayer()
    {
        var bounds = box.bounds;

        float xSpawn = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float zSpawn = UnityEngine.Random.Range(bounds.min.z, bounds.max.z*distanceMultiplier);

        Vector3 spawnPosition = new Vector3(xSpawn, spawnHeight, zSpawn);

        currentObject = Instantiate(
            objectPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}
