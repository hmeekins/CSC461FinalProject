using UnityEngine;

public class RusherSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public Transform cameraRigTarget;

    private GameObject rusher;

    void Update()
    {
        if (GlobalVariables.hasTeleported && rusher == null)
        {
            rusher = Instantiate(
                objectPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            rusher.GetComponent<RusherMovement>().target = cameraRigTarget;
        }
    }
}
