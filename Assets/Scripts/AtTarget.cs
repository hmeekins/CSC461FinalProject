using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtTarget : MonoBehaviour
{
    public bool atTargetZ = false;
    public bool atTargetX = false;
    public string playerPosition;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == "Left")
        {
            if (transform.position.z == GlobalVariables.leftTargetZ)
            {
                atTargetZ = true;
            }
            if (transform.position.x == GlobalVariables.leftTargetX)
            {
                atTargetX = true;
            }
        }
        if (playerPosition == "Right")
        {
            if (transform.position.z == GlobalVariables.rightTargetZ)
            {
                atTargetZ = true;
            }
            if (transform.position.x == GlobalVariables.rightTargetX)
            {
                atTargetX = true;
            }
        }
    }
}
