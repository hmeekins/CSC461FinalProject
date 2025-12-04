using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static int score = 1000;
    public static int highscore = 0;
    public static int downs = 1;
    public static bool ballThrown = false;

    public static Vector3 ballPosition = new Vector3(0,0,0);
    public static float leftTargetZ = 0;

    public static float rightTargetZ = 0;

    public static float leftTargetX = 0;

    public static float rightTargetX = 0;

    private static void Start()
    {
        score = 0;
        ballThrown = false;
        downs = 1;
    }
    
}
