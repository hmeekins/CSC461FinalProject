using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPath : MonoBehaviour
{
    public int points = 25;
    public float timeStep = 0.05f;
    public float minPreviewSpeed = 6f;
    public float previewSmoothing = 20f;

    private LineRenderer path;
    private BallBehaviour ball;
    private Transform handTransform;
    private Vector3 offset = new Vector3(0f, 90f, 20f);
    private Vector3 smoothedVelocity;

    void Start()
    {
        var rig = FindObjectOfType<OVRCameraRig>();
        handTransform = rig.rightHandAnchor;
        path = gameObject.GetComponent<LineRenderer>();
        ball = gameObject.GetComponent<BallBehaviour>();
    }

    void Update()
    {
        if (!ball.IsHoldingBall())
        {
            path.enabled = false;
            return;
        }

        path.enabled = true;
        DrawPath();
    }

    /// <summary>
    /// Retrieves ball position and uses same formula used for throwing to calculate predicted path, if the velocity of the ball
    /// is below the minimum velocity threshold, the minimum velocity is used instead. This prevents path pointing straight down when ball is
    /// not being thrown.
    /// </summary>
    void DrawPath()
    {
        Vector3 position = transform.position;

        Vector3 velocity = ball.GetPredictedVelocity();

        Quaternion aimRot = handTransform.rotation * Quaternion.Euler(offset);
        Vector3 aimDir = (aimRot * Vector3.left).normalized;

        if (velocity.magnitude < minPreviewSpeed)
            velocity = aimDir * minPreviewSpeed;

        smoothedVelocity = Vector3.Lerp(smoothedVelocity, velocity, 1f - Mathf.Exp(-previewSmoothing * Time.deltaTime));

        path.positionCount = points ;

        Vector3 p = position;
        Vector3 v = smoothedVelocity;

        for (int i = 0; i < points; i++)
        {
            path.SetPosition(i, p);
            p = p + v * timeStep;
            v = v + Physics.gravity * timeStep;
        }
    }
}