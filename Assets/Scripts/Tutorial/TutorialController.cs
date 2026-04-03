using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialStep
    {
        GameBasics,
        SpawnBall,
        HitTarget,
        HitMovingTarget,
        Defenders,
        TutorialOver
    }

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;
    public TutorialStep TutorialStep;
}
