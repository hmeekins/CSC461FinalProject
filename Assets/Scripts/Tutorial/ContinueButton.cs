using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public void ChangeStep() 
    {
        if (TutorialController.Instance.TutorialStep == TutorialStep.SpawnBall)
            TutorialController.Instance.FinishZones();
        if (TutorialController.Instance.TutorialStep == TutorialStep.HitMovingTarget)
            TutorialController.Instance.CoverDefenders();
        else if (TutorialController.Instance.TutorialStep == TutorialStep.Defenders)
            TutorialController.Instance.CoverRusher();
        else
            TutorialController.Instance.EndTutorial();
    }
}
