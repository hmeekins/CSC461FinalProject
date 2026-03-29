using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class BallSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    
    private OVRInput.Axis1D triggerAxis;
    public float pressThreshold = 0.8f;
    public Vector3 offset = new Vector3(0f, 90f, 20f);
    private Transform handTransform;
    private OVRInput.Controller controller;
    private GameObject currentObject;
    private Queue<Vector3> _velHistory = new Queue<Vector3>();
    private bool hasReleasedTrigger = true;


    public void Start()
    {
        OVRCameraRig rig = FindObjectOfType<OVRCameraRig>();
        if (!GameFlowController.Instance.LeftHanded)
        {
            controller = OVRInput.Controller.RTouch;
            handTransform = rig.rightHandAnchor;
            triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;
        }
        else
        {
            controller = OVRInput.Controller.LTouch;
            handTransform = rig.leftHandAnchor;
            triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;
        }
        UnityEngine.Debug.Log(controller + " " + triggerAxis);
    }
    private void Update()
    {
        float triggerValue = OVRInput.Get(triggerAxis, controller);

        if (GameFlowController.Instance.State != GameState.WaitingForSnap) 
            return;
        if (triggerValue < pressThreshold)
            hasReleasedTrigger = true;
        if (currentObject == null && triggerValue >= pressThreshold && hasReleasedTrigger == true)
        {   
            SpawnObjectInHand();
            GameFlowController.Instance.StartPlay();
            hasReleasedTrigger = false;
        }

    }
    /// <summary>
    /// Spawns football in player hand
    /// </summary>
    private void SpawnObjectInHand()
    {
        //Adjust base rotation so ball feels more natural in hand
        Quaternion spawnRot = handTransform.rotation * Quaternion.Euler(offset);

        currentObject = Instantiate(
            objectPrefab,
            handTransform.position,
            spawnRot
        );

        var rbs = currentObject.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        _velHistory.Clear();
    }

}
