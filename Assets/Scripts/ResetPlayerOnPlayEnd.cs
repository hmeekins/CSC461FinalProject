using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Locomotion;

public class ResetPlayerOnPlayEnd : MonoBehaviour
{
    [Header("Rig References")]
    public Transform cameraRig;      // ✅ [BuildingBlock] Camera Rig
    public Transform trackingSpace; // ✅ TrackingSpace (child of rig)
    public Transform resetPoint;     // ✅ PlayerSpawnPoint

    [Header("Locomotion")]
    public GameObject locomotor;    // ✅ Locomotor GameObject

    [Header("Facing Direction")]
    public float targetYaw = 180f;  // Which way the player should face

    private void LateUpdate()
    {
        if (!GlobalVariables.runActive)
        {
            // ✅ HARD FREEZE MOVEMENT
            locomotor.SetActive(false);

            // ✅ TELEPORT ROOT (NOT THE HEAD)
            cameraRig.position = resetPoint.position;

            // ✅ ROTATE VIA TRACKING SPACE (THIS IS THE VR FIX)
            Vector3 currentRot = trackingSpace.eulerAngles;
            trackingSpace.rotation = Quaternion.Euler(
                currentRot.x,
                targetYaw,
                currentRot.z
            );
        }
        else
        {
            // ✅ MOVEMENT ENABLED ONLY AFTER PLAY STARTS
            if (!locomotor.activeSelf)
                locomotor.SetActive(true);
        }
    }
}