using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    private float _roundTime = 0f;
    private float _previousTime = 0f;
    private bool _timerStarted = false;

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.PlayRunning && !_timerStarted)
            _timerStarted = true;

        if (GameFlowController.Instance.State != GameState.GameOver && _timerStarted)
        {
            _roundTime += Time.deltaTime;
            GameData.SetRoundDuration((int)System.Math.Round(_roundTime));
            if (_roundTime - _previousTime >= 2f || _previousTime == 0f)
            {
                _previousTime = _roundTime;
            }
        }
    }


}
