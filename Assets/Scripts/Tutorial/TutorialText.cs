using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TMP_Text _tutorial;
    void Update()
    {
        DisplayText();
    }

    private void DisplayText() {
    switch(TutorialController.Instance.TutorialStep)
    {
        case TutorialStep.GameBasics:
            _tutorial.text = "Welcome to Spiral VR! Your goal is to complete passes and rack up the highest score possible. To begin, press and hold the front trigger.";
            break;

        case TutorialStep.SpawnBall:
            _tutorial.text = "Nice! Now throw the ball at the target. Use a natural throwing motion, and release the trigger just like you would release a real ball.";
            break;

        case TutorialStep.HitTarget:
            _tutorial.text = "Great job! In real games, your targets won’t stay still. Try hitting this moving target.";
            break;

        case TutorialStep.HitMovingTarget:
            _tutorial.text = "Excellent work! You're almost ready to play, just one more thing to know.";
            break;

        case TutorialStep.Defenders:
            _tutorial.text = "In a real game, defenders will follow your teammates. If a defender catches the ball, you lose the play. Also watch out for the rusher charging at you or you will be tackled!";
            break;

        case TutorialStep.TutorialOver:
            _tutorial.text = "You're all set! Jump in and start playing Spiral VR. Good luck!";
            break;
    }
}
}
