using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameOnMaxPlays : MonoBehaviour
{
    private int _playNum = 0;
    private bool _gameReset = true;
    void Update()
    {
        if (GameFlowController.Instance.State == GameState.WaitingForSnap && _gameReset == true)
        {
            _gameReset = false;
            _playNum += 1;
        }
        else if (GameFlowController.Instance.State == GameState.Resetting)
            _gameReset = true;
        else if (GameFlowController.Instance.State == GameState.GameOver)
        {
            _playNum = 0;
            _gameReset = true;
        }

        if (_playNum > GameFlowController.Instance.MaxPlays)
        {
            GameFlowController.Instance.EndGame();
        }
    }
}
