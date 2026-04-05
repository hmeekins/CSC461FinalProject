using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public void ChangeStep() 
    {
        if (TutorialController.Instance.TutorialStep == TutorialStep.HitMovingTarget)
            TutorialController.Instance.CoverDefenders();
        else
            TutorialController.Instance.EndTutorial();
    }
}
