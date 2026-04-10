using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private GameObject _target;
    private GameObject _enemy;
    private GameObject _controllerImage;
    private GameObject _continueB;
    private GameObject _startGameB;
    private GameObject _returnB;

    
    void Start()
    {
        _target = GameObject.Find("Target");
        _enemy = GameObject.Find("Enemy");
        _controllerImage = GameObject.Find("ControllerImg");
        _continueB = GameObject.Find("Continue");
        _startGameB = GameObject.Find("StartGame");
        _returnB = GameObject.Find("Return");
    }

    void Update()
    {
        if (TutorialController.Instance.TutorialStep == TutorialStep.GameBasics)
            CheckBall();

        TargetVisible();
        EnemyVisible();
        MenuVisibility();
    }

    private void CheckBall()
    {
        GameObject ball = GameObject.FindWithTag("Ball");
        if (ball != null)
        {
            TutorialController.Instance.BallSpawned();
        }
    }

    private void TargetVisible()
    {
        if(TutorialController.Instance.TutorialStep == TutorialStep.SpawnBall || TutorialController.Instance.TutorialStep == TutorialStep.HitTarget)
            _target.SetActive(true);
        else
            _target.SetActive(false);
    }

    private void EnemyVisible()
    {
        if(TutorialController.Instance.TutorialStep == TutorialStep.Defenders || TutorialController.Instance.TutorialStep == TutorialStep.Rusher)
            _enemy.SetActive(true);
        else
            _enemy.SetActive(false);
    }

    private void MenuVisibility()
    {
        if (TutorialController.Instance.TutorialStep == TutorialStep.GameBasics)
            _controllerImage.SetActive(true);
        else
            _controllerImage.SetActive(false);

        if (TutorialController.Instance.TutorialStep == TutorialStep.HitMovingTarget || TutorialController.Instance.TutorialStep == TutorialStep.Defenders || TutorialController.Instance.TutorialStep == TutorialStep.Rusher)
            _continueB.SetActive(true);
        else
            _continueB.SetActive(false);
        
        if (TutorialController.Instance.TutorialStep == TutorialStep.TutorialOver)
        {
            _startGameB.SetActive(true);
            _returnB.SetActive(true);
        }
        else
        {
            _startGameB.SetActive(false);
            _returnB.SetActive(false);
        }
    }
}
