using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHighScore : MonoBehaviour
{
    void Start()
    {
        GlobalVariables.highscore = PlayerPrefs.GetInt("HIGH_SCORE", 0);
    }
}
