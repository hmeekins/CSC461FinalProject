using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVideo : MonoBehaviour
{
    [SerializeField] private GameObject _videoPlayer;
    
    void Update()
    {
        if (TutorialController.Instance.TutorialStep == TutorialStep.SpawnBall)
            _videoPlayer.SetActive(true);
        else
            _videoPlayer.SetActive(false);
    }
}
