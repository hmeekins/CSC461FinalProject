using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPath : MonoBehaviour
{
    public LineRenderer path;
    private Transform handTransform;
    public Vector3 offset = new Vector3(0f, 90f, 20f);
    public float length = 5f;

    void Start()
    {
        var rig = FindObjectOfType<OVRCameraRig>();
        handTransform = rig.rightHandAnchor;
        path = gameObject.GetComponent<LineRenderer>();
    }
    void Update()
    {

        Quaternion aimRot = handTransform.rotation * Quaternion.Euler(offset);
        Vector3 start = transform.position;
        Vector3 dir = aimRot * Vector3.left;
        path.positionCount = 2;
        
        path.SetPosition(0, start);
        path.SetPosition(1, start + dir * length);
    }
}
