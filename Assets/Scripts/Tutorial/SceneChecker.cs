using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChecker : MonoBehaviour
{
    private GameObject _target;
    
    void Start()
    {
        _target = GameObject.Find("Target");
    }

    void Update()
    {
        if (TutorialController.Instance.TutorialStep == TutorialStep.GameBasics)
            CheckBall();
        TargetVisible();
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
}
