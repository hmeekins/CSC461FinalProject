using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public GameObject locomotor;
    public GameObject canvas;
    public Transform centerEyeAnchor;

    public float spawnDistance = 1.5f;
    public float verticalOffset = -0.1f;

    private bool menuSpawned = false;

    void Start()
    {
        canvas.SetActive(false);
    }

    void Update()
    {
        if (GlobalVariables.downs > 4)
            GameFlowController.Instance.EndGame();

        if (GameFlowController.Instance.State == GameState.GameOver)
        {
            if (!menuSpawned)
            {
                SpawnMenuInFrontOfPlayer();
                locomotor.SetActive(false);
                menuSpawned = true;
            }
        }
        else
        {
            canvas.SetActive(false);
            menuSpawned = false;
        }
    }

    void SpawnMenuInFrontOfPlayer()
    {
        Vector3 headPosition = centerEyeAnchor.position;

        Vector3 forward = centerEyeAnchor.forward;
        forward.y = 0f;
        forward.Normalize();

        if (forward.sqrMagnitude < 0.001f)
        {
            forward = Vector3.forward;
        }

        Vector3 spawnPosition =
            headPosition +
            forward * spawnDistance +
            Vector3.up * verticalOffset;

        canvas.transform.SetParent(null);

        canvas.transform.position = spawnPosition;

        Vector3 lookDirection = headPosition - spawnPosition;
        lookDirection.y = 0f;

        canvas.transform.rotation = Quaternion.LookRotation(-lookDirection);

        canvas.SetActive(true);
    }
}