using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPath : MonoBehaviour
{
    public int points = 40;
    public float timeStep = 0.05f;

    public float minPreviewSpeed = 6f;
    public float previewSmoothing = 20f;
    public float directionSmoothing = 18f;

    public float pumpHoldTime = 0.3f;
    public float decayPerSecond = 10f;

    public Gradient throwColorGradient;
    public float maxColorSpeed = 20f;

    public float minWidth = 0.01f;
    public float maxWidth = 0.05f;

    private LineRenderer path;
    private BallBehaviour ball;
    private Transform handTransform;
    private Vector3 offset = new Vector3(0f, 90f, 20f);

    private Vector3 smoothedDir;
    private float smoothedSpeed;
    private float latchedSpeed;
    private float latchedTime;
    private bool isPreviewing;
    private bool wasHoldingBall;
    private Vector3 thrownPosition;
    private Vector3 thrownVelocity;

    void Start()
    {
        var rig = FindObjectOfType<OVRCameraRig>();
        handTransform = rig.rightHandAnchor;
        path = gameObject.GetComponent<LineRenderer>();
        ball = gameObject.GetComponent<BallBehaviour>();
    }

    void Update()
    {
        bool isHoldingBall = ball.IsHoldingBall();

        if (wasHoldingBall && !isHoldingBall)
        {
            thrownPosition = transform.position;
            thrownVelocity = ball.GetPredictedVelocity();
        }

        wasHoldingBall = isHoldingBall;

        if (GameFlowController.Instance.Variation == GameVariation.Variation4 ||
        GameFlowController.Instance.Variation == GameVariation.Variation5 ||
        GameFlowController.Instance.Variation == GameVariation.Variation6)
        {
            if (!isHoldingBall)
            {
                path.enabled = true;
                DrawThrownPath();
                return;
            }

            path.enabled = true;
            DrawPath();
        }
    }

    void UpdatePathVisuals(float speed)
    {
        float t = Mathf.InverseLerp(minPreviewSpeed, maxColorSpeed, speed);
        Color c = throwColorGradient.Evaluate(t);
        float w = Mathf.Lerp(minWidth, maxWidth, t);

        path.startColor = c;
        path.endColor = c;
        path.startWidth = w;
        path.endWidth = w;
    }

    void DrawPath()
    {
        Vector3 position = transform.position;

        Quaternion aimRot = handTransform.rotation * Quaternion.Euler(offset);
        Vector3 aimDir = (aimRot * Vector3.left).normalized;

        float currentSpeed = ball.GetPredictedVelocity().magnitude;
        currentSpeed = Mathf.Max(currentSpeed, minPreviewSpeed);

        float now = Time.time;

        if (currentSpeed >= latchedSpeed)
        {
            latchedSpeed = currentSpeed;
            latchedTime = now;
        }
        else
        {
            if (now - latchedTime > pumpHoldTime)
                latchedSpeed = Mathf.Max(minPreviewSpeed, latchedSpeed - decayPerSecond * Time.deltaTime);
        }

        float aSpeed = 1f - Mathf.Exp(-previewSmoothing * Time.deltaTime);
        float aDir = 1f - Mathf.Exp(-directionSmoothing * Time.deltaTime);

        if (smoothedDir == Vector3.zero)
            smoothedDir = aimDir;
        else
            smoothedDir = Vector3.Slerp(smoothedDir, aimDir, aDir);

        smoothedSpeed = Mathf.Lerp(smoothedSpeed, latchedSpeed, aSpeed);
        UpdatePathVisuals(smoothedSpeed);

        Vector3 p = position;
        Vector3 v = smoothedDir * smoothedSpeed;

        int count = points + 1;
        path.positionCount = count;

        path.SetPosition(0, p);

        for (int i = 0; i < points; i++)
        {
            Vector3 nextP = p + v * timeStep + 0.5f * Physics.gravity * timeStep * timeStep;
            Vector3 nextV = v + Physics.gravity * timeStep;

            path.SetPosition(i + 1, nextP);

            p = nextP;
            v = nextV;
        }
    }

    void DrawThrownPath()
    {
        Vector3 p = thrownPosition;
        Vector3 v = thrownVelocity;
        UpdatePathVisuals(v.magnitude);

        int count = points + 1;
        path.positionCount = count;

        path.SetPosition(0, p);

        for (int i = 0; i < points; i++)
        {
            Vector3 nextP = p + v * timeStep + 0.5f * Physics.gravity * timeStep * timeStep;
            Vector3 nextV = v + Physics.gravity * timeStep;

            path.SetPosition(i + 1, nextP);

            p = nextP;
            v = nextV;
        }
    }
}