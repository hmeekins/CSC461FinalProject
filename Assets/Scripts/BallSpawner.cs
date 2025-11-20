using UnityEngine;

/// <summary>
/// Spawns an object in the player's hand when the controller trigger is pressed.
/// On trigger release, the object is dropped (unparented) and physics is enabled,
/// so the player can throw it.
/// </summary>
public class SpawnInHandOnTrigger : MonoBehaviour
{
    public GameObject objectPrefab;

    public Transform handTransform;

    public OVRInput.Controller controller;

    public OVRInput.Axis1D triggerAxis;

    public float pressThreshold;

    public float releaseThreshold;

    private GameObject _currentObject;
    private bool _isHolding;
    private Quaternion _originalRotation;

    private void Start()
    {
        _originalRotation = objectPrefab.transform.rotation;
    }

    private void Update()
    {
        float triggerValue = OVRInput.Get(triggerAxis, controller);

        // Trigger pressed: spawn in hand if not holding anything
        if (!_isHolding && triggerValue >= pressThreshold)
        {
            SpawnObjectInHand();
        }

        // Trigger released: object
        if (_isHolding && triggerValue <= releaseThreshold)
        {
            ReleaseObject();
        }
    }

    private void SpawnObjectInHand()
    {
        // Spawn at hand position & rotation
        _currentObject = Instantiate(
            objectPrefab,
            handTransform.position,
            objectPrefab.transform.rotation
        );
        _isHolding = true;
    }

    private void ReleaseObject()
    {
        if (_currentObject == null) return;
        _currentObject = null;
        _isHolding = false;
    }
}