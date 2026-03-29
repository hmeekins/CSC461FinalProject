using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using TMPro;

public class LivesText : MonoBehaviour
{
    public TMP_Text downsText;

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.StartGame || GameFlowController.Instance.IgnoreDowns)
            downsText.text = "";
        else if (GameFlowController.Instance.State == GameState.WaitingForSnap)
            downsText.text = "Down: " + GlobalVariables.downs.ToString();
    }
}
