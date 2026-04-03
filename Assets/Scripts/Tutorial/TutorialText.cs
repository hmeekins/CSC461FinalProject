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
                _tutorial.text = "Welcome to SpiralVR, your goal is to complete passes to get as high of a score as you can. To start press and hold the front trigger.";
                break;
            case TutorialStep.SpawnBall:
                _tutorial.text = "Now throw the ball at that target. Throw like you normally would and release the trigger the same way you would release the ball.";
                break;
            case TutorialStep.HitTarget:
                _tutorial.text = "Great Job! However, players will be moving in an actual game. Try hitting this moving target.";
                break;
            case TutorialStep.HitMovingTarget:
                _tutorial.text = "Fantastic! You're about ready to play, theres just one more thing I need to mention.";
                break;
            case TutorialStep.Defenders:
                _tutorial.text = "In an actual game you will have defenders following your teammates, if they recieve the ball you lose, there is also a rusher that will run at you, avoid being tackled.";
                break;
            case TutorialStep.TutorialOver:
                _tutorial.text = "You're now ready to begin playing!";
                break;
        }
    }
}
