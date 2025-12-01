using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeammateMovement : MonoBehaviour
{
    public string playerPosition;

    public float speed;

    private Vector3 targetPosition;
    private AtTarget atTarget;

    private Vector3 pos;

    private bool atTargetZ;


    void Start()
    {
        atTarget = GetComponent<AtTarget>();
    }
    private void Update()
    {
        if (GameFlowController.Instance.State == GameState.PlayRunning)
        {
            Run();
            checkProgress();
            
        }
    }
    /// <summary>
    /// Finds target points for player to run to. Player will first run to designated z value and then run to designated x
    /// </summary>
    private void Run()
    {
        if (!atTargetZ)
        {
            pos = gameObject.transform.position;
            targetPosition = new Vector3(pos.x, pos.y, atTarget.targetZ);

            gameObject.transform.position = Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }
        if (atTarget.atTargetZ)
        {
            pos = gameObject.transform.position;
            targetPosition = new Vector3(atTarget.targetX, pos.y, pos.z);

            Vector3 moveDir = targetPosition - pos;
            if (moveDir != Vector3.zero)
                gameObject.transform.rotation = Quaternion.LookRotation(moveDir);

           gameObject.transform.position = Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        }
    }
    
    private void checkProgress()
    {
        if (playerPosition == "Left")
        {
            if (transform.position.z == GlobalVariables.leftTargetZ)
                atTargetZ = true;
            if (transform.position.x == GlobalVariables.leftTargetX)
                Destroy(gameObject);
        }
        else
        {
            if (transform.position.z == GlobalVariables.rightTargetZ)
                atTargetZ = true;
            if (transform.position.x == GlobalVariables.rightTargetX)
                Destroy(gameObject);
        }
    }
}

