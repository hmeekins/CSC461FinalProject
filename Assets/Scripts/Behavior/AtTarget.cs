using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtTarget : MonoBehaviour
{
    public bool atTargetZ = false;
    public bool atTargetX = false;
    public string playerPosition;

    public float targetZ;
    public float targetX;

    TeammateMovement teammateMovement;

    void Start()
    {
        teammateMovement = GetComponent<TeammateMovement>();
    }

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.PlayRunning)
            checkProgress();
    }

    /// <summary>
    /// Checks to see if player position is at target x or z.
    /// </summary>
    private void checkProgress()
    {
        if (teammateMovement.playerPosition == "Left")
        {
            targetZ = GlobalVariables.leftTargetZ;
            targetX = GlobalVariables.leftTargetX;
            if (transform.position.z == GlobalVariables.leftTargetZ)
                atTargetZ = true;
            if (transform.position.x == GlobalVariables.leftTargetX)
                atTargetX = true;
        }
        else
        {
            targetZ = GlobalVariables.rightTargetZ;
            targetX = GlobalVariables.rightTargetX;
            if (transform.position.z == GlobalVariables.rightTargetZ)
                atTargetZ = true;
            if (transform.position.x == GlobalVariables.rightTargetX)
                atTargetX = true;
        }
    }
}
