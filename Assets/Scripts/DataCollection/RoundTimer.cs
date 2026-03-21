using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    private float _roundTime = 0.0f;
    private bool _timerStarted = false;

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.WaitingForSnap && !_timerStarted)
            _timerStarted = true;

        if (GameFlowController.Instance.State != GameState.GameOver && _timerStarted)
            _roundTime += Time.deltaTime;
            GameData.SetRoundDuration((int)System.Math.Round(_roundTime));
    }
}
