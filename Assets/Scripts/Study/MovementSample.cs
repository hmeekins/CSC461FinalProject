using UnityEngine;

public class MovementSample
{
    public int PlayNum {get; private set;}
    public string Timestamp { get; private set; }

    public Vector3 HeadPosition { get; private set; }
    public Quaternion HeadRotation { get; private set; }

    public Vector3 LeftHandPosition { get; private set; }
    public Quaternion LeftHandRotation { get; private set; }

    public Vector3 RightHandPosition { get; private set; }
    public Quaternion RightHandRotation { get; private set; }

    public int Variation { get; private set; }

    public MovementSample(int playNum, string timestamp, Vector3 headPosition, Quaternion headRotation, Vector3 leftHandPosition, Quaternion leftHandRotation, Vector3 rightHandPosition, Quaternion rightHandRotation)
    {
        PlayNum = playNum;
        Timestamp = timestamp;
        HeadPosition = headPosition;
        HeadRotation = headRotation;
        LeftHandPosition = leftHandPosition;
        LeftHandRotation = leftHandRotation;
        RightHandPosition = rightHandPosition;
        RightHandRotation = rightHandRotation;
    }
}