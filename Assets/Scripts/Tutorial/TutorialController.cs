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

    private void Awake()
    {
        Instance = this;
    }
    
    void Start() 
    {
        GameFlowController.Instance.EnterWaitingForSnap();
    }

    public void StartTutorial()
    {
        Instance.TutorialStep = TutorialStep.GameBasics;
    }

    public void BallSpawned()
    {
        Instance.TutorialStep = TutorialStep.SpawnBall;
    }

    public void HitFirstTarget()
    {
        Instance.TutorialStep = TutorialStep.HitTarget;
    }

    public void HitSecondTarget()
    {
        Instance.TutorialStep = TutorialStep.HitMovingTarget;
    }

    public void CoverDefenders()
    {
        Instance.TutorialStep = TutorialStep.Defenders;
    }

    public void EndTutorial()
    {
        Instance.TutorialStep = TutorialStep.TutorialOver;
    }
}
