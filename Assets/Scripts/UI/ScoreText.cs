using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using TMPro;

public class ScoreText : MonoBehaviour
{
    public TMP_Text scoreText;

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.StartGame)
            scoreText.text = "";
        else
            scoreText.text = "Score:\n" + GlobalVariables.score.ToString();
    }
}
