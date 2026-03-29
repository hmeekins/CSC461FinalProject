using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _releaseThreshold = 0.2f;
    [SerializeField] private int _velocitySamples = 10;
    [SerializeField] private float _velocityMultiplier = 2f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 90f, 20f);

    private Transform _handTransform;
    private OVRInput.Controller _controller;
    private OVRInput.Axis1D _triggerAxis;
    private GameObject _currentObject;
    private Queue<Vector3> _velHistory = new Queue<Vector3>();
    private Transform _trackingSpace;

    private void Start()
    {
        _currentObject = gameObject;
        var rig = FindObjectOfType<OVRCameraRig>();
        _trackingSpace = rig.trackingSpace;
        if (!GameFlowController.Instance.LeftHanded)
        {
            _controller = OVRInput.Controller.RTouch;
            _handTransform = rig.rightHandAnchor;
            _triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;
        }
        else
        {
            _controller = OVRInput.Controller.LTouch;
            _handTransform = rig.leftHandAnchor;
            _triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;
        }
    }

    void Update()
    {
        //Float value representing the press amount of trigger
        float triggerValue = OVRInput.Get(_triggerAxis, _controller);
        RecordControllerVelocity();

        if (_currentObject != null && triggerValue <= _releaseThreshold)
        {
                _audioSource.Play();
                ReleaseObject();
                _currentObject = null;
        }
        if (_currentObject != null)
        {
            hold();
        }
    }
    
    private void hold()
    {
        _currentObject.transform.position = _handTransform.position;
        _currentObject.transform.rotation = _handTransform.rotation * Quaternion.Euler(_offset);
    }

    /// <summary>
    /// Records velocity frame and puts it in queue, dequeues oldest frame once queue has exceeded designated limit
    /// </summary>
    private void RecordControllerVelocity()
    {
        Vector3 vel = OVRInput.GetLocalControllerVelocity(_controller);
        
        _velHistory.Enqueue(vel);

        while (_velHistory.Count > _velocitySamples)
            _velHistory.Dequeue();
    }

    /// <summary>
    /// Calculates the average of all velocities in the queue
    /// </summary>
    /// <returns>Average of all velocities in queue</returns>
    private Vector3 GetAveragedVelocity()
    {
        if (_velHistory.Count == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (var v in _velHistory)
            sum += v;

        return sum / _velHistory.Count;
    }

    /// <summary>
    /// Applys velocity to a released object and turns physics back on
    /// </summary>
    private void ReleaseObject()
    {
        GlobalVariables.ballPosition = _currentObject.transform.position;
        GlobalVariables.ballThrown = true;
        Vector3 throwVel = GetAveragedVelocity() * _velocityMultiplier;

        Rigidbody rb = _currentObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.velocity = _trackingSpace.TransformDirection(throwVel);

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        GameData.RegisterPass();
    }

    public bool IsHoldingBall()
    {
        return _currentObject != null;
    }

    public Vector3 GetPredictedVelocity()
    {
        Vector3 throwVel = GetAveragedVelocity() * _velocityMultiplier;

        return _trackingSpace.TransformDirection(throwVel);
    }
}
