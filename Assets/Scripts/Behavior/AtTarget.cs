using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtTarget : MonoBehaviour
{
    public bool AtTargetZ = false;
    public bool AtTargetX = false;
    public string PlayerPosition;

    public float TargetZ;
    public float TargetX;

    private TeammateMovement _teammateMovement;

    void Start()
    {
        _teammateMovement = GetComponent<TeammateMovement>();
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
        if (_teammateMovement.PlayerPosition == "Left")
        {
            TargetZ = GlobalVariables.leftTargetZ;
            TargetX = GlobalVariables.leftTargetX;
            if (transform.position.z == GlobalVariables.leftTargetZ)
                AtTargetZ = true;
            if (transform.position.x == GlobalVariables.leftTargetX)
                AtTargetX = true;
        }
        else
        {
            TargetZ = GlobalVariables.rightTargetZ;
            TargetX = GlobalVariables.rightTargetX;
            if (transform.position.z == GlobalVariables.rightTargetZ)
                AtTargetZ = true;
            if (transform.position.x == GlobalVariables.rightTargetX)
                AtTargetX = true;
        }
    }
}
