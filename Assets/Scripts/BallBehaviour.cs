using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform handTransform;
    public OVRInput.Controller controller;
    public OVRInput.Axis1D triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;
    public float releaseThreshold = 0.2f;
    public int velocitySamples = 10;
    public float velocityMultiplier = 2f;
    public Vector3 offset = new Vector3(0f, 90f, 20f);
    private GameObject currentObject;
    private Queue<Vector3> _velHistory = new Queue<Vector3>();
    private Transform trackingSpace;

    private void Start()
    {
        currentObject = gameObject;
        var rig = FindObjectOfType<OVRCameraRig>();
        trackingSpace = rig.trackingSpace;
        handTransform = rig.rightHandAnchor;
    }

    void Update()
    {
        //Float value representing the press amount of trigger
        float triggerValue = OVRInput.Get(triggerAxis, controller);
        RecordControllerVelocity();

        if (currentObject != null && triggerValue <= releaseThreshold)
        {
                audioSource.Play();
                ReleaseObject();
                currentObject = null;
        }
        if (currentObject != null)
        {
            hold();
        }
    }
    private void hold()
    {
        currentObject.transform.position = handTransform.position;
        currentObject.transform.rotation = handTransform.rotation * Quaternion.Euler(offset);
    }

    /// <summary>
    /// Records velocity frame and puts it in queue, dequeues oldest frame once queue has exceeded designated limit
    /// </summary>
    private void RecordControllerVelocity()
    {
        Vector3 vel = OVRInput.GetLocalControllerVelocity(controller);
        _velHistory.Enqueue(vel);

        while (_velHistory.Count > velocitySamples)
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
        GlobalVariables.ballPosition = currentObject.transform.position;
        GlobalVariables.ballThrown = true;
        Vector3 throwVel = GetAveragedVelocity() * velocityMultiplier;

        Rigidbody rb = currentObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        if (trackingSpace != null)
            rb.velocity = trackingSpace.TransformDirection(throwVel);
        else
            rb.velocity = throwVel;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }
}
