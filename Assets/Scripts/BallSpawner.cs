using UnityEngine;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    [Header("Ball Setup")]
    public GameObject objectPrefab;
    public AudioSource audioSource;
    public Transform handTransform;

    [Header("Controller Input")]
    public OVRInput.Controller controller;
    public OVRInput.Axis1D triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;
    public float pressThreshold = 0.8f;
    public float releaseThreshold = 0.2f;

    [Header("Throw Settings")]
    public int velocitySamples = 10;
    public float velocityMultiplier = 2f;
    public Vector3 offset = new Vector3(0f, 90f, 20f);

    private GameObject _currentObject;
    private Queue<Vector3> _velHistory = new Queue<Vector3>();
    private Transform trackingSpace;

    private bool ballSpawned;

    private void Start()
    {
        var rig = FindObjectOfType<OVRCameraRig>();
        if (rig != null)
        {
            trackingSpace = rig.trackingSpace;
        }
    }

    private void Update()
    {
        float triggerValue = OVRInput.Get(triggerAxis, controller);

        RecordControllerVelocity();

        // ✅ SPAWN BALL (ENABLE MOVEMENT HERE)
        if (GlobalVariables.hasTeleported && _currentObject == null && triggerValue >= pressThreshold && !ballSpawned)
        {
            SpawnObjectInHand();
            ballSpawned = true;
        }
        if (triggerValue <= pressThreshold)
        {
            ballSpawned = false;
        }

        // ✅ RELEASE BALL
        if (GlobalVariables.isHolding && triggerValue <= releaseThreshold)
        {
            if (audioSource != null)
                audioSource.Play();

            ReleaseObject();
        }

        // ✅ HOLD BALL IN HAND
        if (GlobalVariables.isHolding && _currentObject != null)
        {
            _currentObject.transform.position = handTransform.position;
            _currentObject.transform.rotation =
                handTransform.rotation * Quaternion.Euler(offset);
        }
        if (GlobalVariables.runEnd)
        {
            Destroy(_currentObject);
        }
    }

    // ---------------------------------------------------------

    private void RecordControllerVelocity()
    {
        Vector3 vel = OVRInput.GetLocalControllerVelocity(controller);
        _velHistory.Enqueue(vel);

        while (_velHistory.Count > velocitySamples)
            _velHistory.Dequeue();
    }

    private Vector3 GetAveragedVelocity()
    {
        if (_velHistory.Count == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (var v in _velHistory)
            sum += v;

        return sum / _velHistory.Count;
    }

    // ---------------------------------------------------------

    private void SpawnObjectInHand()
    {
        if (objectPrefab == null || handTransform == null)
            return;

        Quaternion spawnRot =
            handTransform.rotation * Quaternion.Euler(offset);

        _currentObject = Instantiate(
            objectPrefab,
            handTransform.position,
            spawnRot
        );

        var rbs = _currentObject.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        _velHistory.Clear();
        GlobalVariables.isHolding = true;
    }

    // ---------------------------------------------------------

    private void ReleaseObject()
    {
        if (_currentObject == null)
            return;

        Vector3 throwVel = GetAveragedVelocity() * velocityMultiplier;

        Rigidbody rb = _currentObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        // ✅ THROW IN WORLD SPACE (NOT PLAYER SPACE)
        if (trackingSpace != null)
            rb.velocity = trackingSpace.TransformDirection(throwVel);
        else
            rb.velocity = throwVel;

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY;

        GlobalVariables.isHolding = false;
        _currentObject = null;
    }
}
