using UnityEngine;
using System.Collections.Generic;

public class SpawnInHandOnTrigger : MonoBehaviour
{
    public GameObject objectPrefab;
    public AudioSource audioSource;

    public Transform handTransform;

    public OVRInput.Controller controller;

    // float value representing how far the trigger has been pressed (0-1)
    public OVRInput.Axis1D triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;
    public float pressThreshold = 0.8f;
    public float releaseThreshold = 0.2f;

    // Number of frames of velocity to average on release
    public int velocitySamples = 10;

    public float velocityMultiplier;

    // How much to rotate the ball relative to the hand (in degrees)
    public Vector3 offset = new Vector3(0f, 90f, 0f);

    private GameObject _currentObject;

    private Queue<Vector3> _velHistory = new Queue<Vector3>();

    private void Update()
    {
        float triggerValue = OVRInput.Get(triggerAxis, controller);

        // Always record controller velocity while active
        RecordControllerVelocity();

        // Spawn
        if (_currentObject == null && triggerValue >= pressThreshold)
        {
            SpawnObjectInHand();
        }

        // Release
        if (GlobalVariables.isHolding && triggerValue <= releaseThreshold)
        {
            if (audioSource != null)
                audioSource.Play();

            ReleaseObject();
        }

        // Keep ball in hand
        if (GlobalVariables.isHolding && _currentObject != null)
        {
            _currentObject.transform.position = handTransform.position;

            // Apply hand rotation + offset
            Quaternion offsetRot = Quaternion.Euler(offset);
            _currentObject.transform.rotation = handTransform.rotation * offsetRot;
        }
    }

    /// <summary>
    /// Adds current velocity of controller to a queue, if number of velocity samples in queue is greater then specified value,
    /// the oldest velocity sample is removed
    /// </summary>
    private void RecordControllerVelocity()
    {
        // Get controller velocity from Oculus
        Vector3 Vel = OVRInput.GetLocalControllerVelocity(controller);

        _velHistory.Enqueue(Vel);

        while (_velHistory.Count > velocitySamples)
        {
            _velHistory.Dequeue();
        }
    }

    /// <summary>
    /// Adds all velocities in queue and divides it by number of items in queue.
    /// </summary>
    /// <returns>The average velocity</returns>
    private Vector3 GetAveragedVelocity()
    {
        if (_velHistory.Count == 0) return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (var v in _velHistory)
        {
            sum += v;
        }
        return sum / _velHistory.Count;
    }

    /// <summary>
    /// Spawns object in hand of player, turns off objects physics and clears velocity history
    /// </summary>
    private void SpawnObjectInHand()
    {
        if (objectPrefab == null || handTransform == null) return;

        // Use same rotation logic as while holding so it doesn't pop
        Quaternion offsetRot = Quaternion.Euler(offset);
        Quaternion spawnRot = handTransform.rotation * offsetRot;

        _currentObject = Instantiate(
            objectPrefab,
            handTransform.position,
            spawnRot
        );

        // While held, control position manually, so keep physics kinematic
        var rbs = _currentObject.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        _velHistory.Clear(); // clear old throw data
        GlobalVariables.isHolding = true;
    }

    /// <summary>
    /// Gets average velocity and applys that with a multiplier to the ball. Turns rigidbody physics back on
    /// </summary>
    private void ReleaseObject()
    {
        if (_currentObject == null) return;

        Vector3 throwVel = GetAveragedVelocity() * velocityMultiplier;

        Rigidbody rb = _currentObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.velocity = throwVel;

        //Freeze X/Y
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        GlobalVariables.isHolding = false;
    }
}
