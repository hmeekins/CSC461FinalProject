using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public enum TutorialStep
    {
        GameBasics,
        SpawnBall,
        HitTarget,
        HitMovingTarget,
        Defenders,
        TutorialOver
    }
}
