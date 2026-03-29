using System;
using UnityEngine;

public class MovementRecorder : MonoBehaviour
{
    public OVRCameraRig cameraRig;
    private float _roundTime = 0f;
    private float _previous = 0f;
    private bool _timerStarted = false;
    private bool _reset = false;
    
    private float _interval = 2f;
    public int PlayNum {get; private set;} = 1;


    void Start()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>();
    }

    void Update()
    {
        if (GameFlowController.Instance.State == GameState.Resetting && !_reset)
        {
            _timerStarted = false;
            _previous = 0;
            _roundTime = 0;
            PlayNum += 1;
            _reset = true;
        }
        else if (GameFlowController.Instance.State == GameState.GameOver)
        {
            _timerStarted = false;
            _previous = 0;
            _roundTime = 0;
            PlayNum = 1;
        }
        
        if (GameFlowController.Instance.State == GameState.PlayRunning && !_timerStarted)
        {
            _timerStarted = true;
            _reset = false;
        }
            
        if (_timerStarted)
        {
            _roundTime += Time.deltaTime;
            if (_roundTime - _previous >= _interval || _previous == 0f)
            {
                _previous = _roundTime;
                RecordMovementSample();
            }
        }
    }

    public void RecordMovementSample()
    {
        Transform head = cameraRig.centerEyeAnchor;
        Transform leftHand = cameraRig.leftHandAnchor;
        Transform rightHand = cameraRig.rightHandAnchor;

        Vector3 headPos = head.position;
        Quaternion headRot = head.rotation;

        Vector3 leftPos = leftHand.position;
        Quaternion leftRot = leftHand.rotation;

        Vector3 rightPos = rightHand.position;
        Quaternion rightRot = rightHand.rotation;
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Debug.Log(
            $"[MovementRecorder]\n" +
            $"Play #: {PlayNum}" +
            $"Time: {timestamp}\n" +
            $"Head Pos: {headPos} | Head Rot: {headRot}\n" +
            $"Left Hand Pos: {leftPos} | Left Hand Rot: {leftRot}\n" +
            $"Right Hand Pos: {rightPos} | Right Hand Rot: {rightRot}"
        );
        GameData.AddMovementSample(new MovementSample(PlayNum, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), headPos, headRot, leftPos, leftRot, rightPos, rightRot));
    }
}