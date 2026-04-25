using UnityEngine;

public class BallPath : MonoBehaviour
{
    [Header("Trajectory")]
    [SerializeField] private int _points = 40;
    [SerializeField] private float _timeStep = 0.05f;

    [Header("Pullback Preview")]
    [SerializeField] private float _maxPreviewSpeed = 20f;

    [Header("Throw Detection")]
    [SerializeField] private float _minThrowSpeed = 1.5f;
    [SerializeField] private float _maxThrowSpeed = 6f;

    [Header("Visuals")]
    [SerializeField] private Gradient _throwColorGradient;
    [SerializeField] private float _maxColorSpeed = 20f;

    [SerializeField] private float _minWidth = 0.01f;
    [SerializeField] private float _maxWidth = 0.05f;
    [SerializeField] private float _maxWidthBoost = 1.8f;
    [SerializeField] private float _minArcLift = 0.05f;
    [SerializeField] private float _maxArcLift = 0.22f;

    [Header("Pullback")]
    [SerializeField] private float _pullbackExponent = 2.5f;
    [SerializeField] private float _backStart = 0.00f;
    [SerializeField] private float _backMax = 0.20f;
    [SerializeField] private float _offset = 0f;

    private LineRenderer _path;
    private BallBehaviour _ball;
    private Transform _handTransform;
    private Transform _headTransform;

    private bool _wasHoldingBall;
    private Vector3 _thrownPosition;
    private Vector3 _thrownVelocity;

    void Start()
    {
        var rig = FindObjectOfType<OVRCameraRig>();

        if (!GlobalVariables.leftHanded)
            _handTransform = rig.rightHandAnchor;
        else
            _handTransform = rig.leftHandAnchor;

        _headTransform = rig.centerEyeAnchor;

        _path = GetComponent<LineRenderer>();
        _ball = GetComponent<BallBehaviour>();
    }

    void Update()
    {
        bool isHoldingBall = _ball.IsHoldingBall();

        if (_wasHoldingBall && !isHoldingBall)
        {
            _thrownPosition = transform.position;
            _thrownVelocity = _ball.GetPredictedVelocity();
        }

        _wasHoldingBall = isHoldingBall;

        if (!ShouldShowPath())
        {
            _path.enabled = false;
            return;
        }

        if (isHoldingBall)
        {
            DrawHeldPath();
        }
        else
        {
            DrawThrownPath();
        }
    }

    bool ShouldShowPath()
    {
        GameVariation variation = GameFlowController.Instance.Variation;

        return variation == GameVariation.Variation4 ||
               variation == GameVariation.Variation5 ||
               variation == GameVariation.Variation6;
    }

    void DrawHeldPath()
    {
        Vector3 startPosition = transform.position;

        Vector3 realVelocity = _ball.GetPredictedVelocity();
        float realSpeed = realVelocity.magnitude;

        float backAmount = GetBackAmount();

        Vector3 headFlat = Vector3.ProjectOnPlane(_headTransform.forward, Vector3.up).normalized;
        if (headFlat == Vector3.zero)
            headFlat = Vector3.forward;

        float curvedBackAmount = 1f - Mathf.Pow(1f - backAmount, _pullbackExponent);

        float arcLift = Mathf.Lerp(_minArcLift, _maxArcLift, curvedBackAmount);
        float verticalAim = _headTransform.forward.y + arcLift;

        Vector3 direction = new Vector3(headFlat.x, verticalAim, headFlat.z).normalized;

        float pullbackSpeed = _maxPreviewSpeed * curvedBackAmount;
        float throwConfidence = Mathf.InverseLerp(_minThrowSpeed, _maxThrowSpeed, realSpeed);
        float finalSpeed = Mathf.Lerp(pullbackSpeed, realSpeed, throwConfidence);

        Vector3 previewVelocity = direction * finalSpeed;
        float confidence = Mathf.Clamp01(Mathf.Max(backAmount, throwConfidence));

        if (confidence <= 0.01f || previewVelocity.sqrMagnitude <= 0.0001f)
        {
            _path.enabled = false;
            return;
        }

        _path.enabled = true;

        UpdatePathVisuals(previewVelocity.magnitude, confidence);
        DrawTrajectory(startPosition, previewVelocity);
    }

    void DrawThrownPath()
    {
        _path.enabled = true;
        UpdatePathVisuals(_thrownVelocity.magnitude, 1f);
        DrawTrajectory(_thrownPosition, _thrownVelocity);
    }

    float GetBackAmount()
    {
        Vector3 flatForward = Vector3.ProjectOnPlane(_headTransform.forward, Vector3.up).normalized;

        if (flatForward == Vector3.zero)
            return 0f;

        Vector3 toHand = _handTransform.position - _headTransform.position;
        float pullBackDistance = Vector3.Dot(-flatForward, toHand) + _offset;

        return Mathf.Clamp01(
            Mathf.InverseLerp(_backStart, _backMax, pullBackDistance)
        );
    }

    void DrawTrajectory(Vector3 startPosition, Vector3 startVelocity)
    {
        Vector3 position = startPosition;
        Vector3 velocity = startVelocity;

        _path.positionCount = _points + 1;
        _path.SetPosition(0, position);

        for (int i = 0; i < _points; i++)
        {
            Vector3 nextPosition = position + velocity * _timeStep + 0.5f * Physics.gravity * _timeStep * _timeStep;
            Vector3 nextVelocity = velocity + Physics.gravity * _timeStep;

            _path.SetPosition(i + 1, nextPosition);

            position = nextPosition;
            velocity = nextVelocity;
        }
    }

    void UpdatePathVisuals(float speed, float confidence)
    {
        float t = Mathf.InverseLerp(0f, _maxColorSpeed, speed);
        Color color = _throwColorGradient.Evaluate(t);

        float alpha = confidence;
        color.a *= alpha;

        float baseWidth = Mathf.Lerp(_minWidth, _maxWidth, t);
        float widthBoost = Mathf.Lerp(0.7f, _maxWidthBoost, confidence);
        float finalWidth = baseWidth * widthBoost;

        _path.startColor = color;
        _path.endColor = color;
        _path.startWidth = finalWidth;
        _path.endWidth = finalWidth;
    }
}