using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreText : MonoBehaviour
{
    public TMP_Text highscoreText;

    void Update()
    {
            highscoreText.text = "Highscore:\n" + GlobalVariables.highscore.ToString();
    }
}
