using UnityEngine;

public class MovementSample
{
    public string Timestamp { get; private set; }

    public Vector3 HeadPosition { get; private set; }
    public Vector3 HeadRotation { get; private set; }

    public Vector3 LeftHandPosition { get; private set; }
    public Vector3 LeftHandRotation { get; private set; }

    public Vector3 RightHandPosition { get; private set; }
    public Vector3 RightHandRotation { get; private set; }

    public int Variation { get; private set; }

    public MovementSample(string timestamp, Vector3 headPosition, Vector3 headRotation, Vector3 leftHandPosition, Vector3 leftHandRotation, Vector3 rightHandPosition, Vector3 rightHandRotation, int variation)
    {
        Timestamp = timestamp;
        HeadPosition = headPosition;
        HeadRotation = headRotation;
        LeftHandPosition = leftHandPosition;
        LeftHandRotation = leftHandRotation;
        RightHandPosition = rightHandPosition;
        RightHandRotation = rightHandRotation;
        Variation = variation;
    }
}