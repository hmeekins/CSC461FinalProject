using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static int score = 0;
    public static int highscore;
    public static int downs = 1;
    public static bool ballThrown = false;
    public static bool leftHanded = false;
    public static int successfulPasses = 0;
    public static Vector3 ballPosition = new Vector3(0,0,0);
    public static float leftTargetZ = 0;

    public static float rightTargetZ = 0;

    public static float leftTargetX = 0;

    public static float rightTargetX = 0;

    public static bool teammateCaught = false;
    public static bool miss = false;
}
