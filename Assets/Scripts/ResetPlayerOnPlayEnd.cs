using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Locomotion;

public class ResetPlayerOnPlayEnd : MonoBehaviour
{
    [Header("Rig References")]
    public Transform cameraRig;
    public Transform trackingSpace;
    public Transform resetPoint;

    [Header("Locomotion")]
    public GameObject locomotor;

    [Header("Facing Direction")]
    public float targetYaw = 180f;

    private OVRScreenFade fade;

    private bool hasEverRun = false;   // ✅ blocks first-spawn fade
    private bool isResetting = false;
    private float fadeStartTime;
    private bool hasTeleported = false;

    private void Start()
    {
        fade = FindObjectOfType<OVRScreenFade>();
    }

    private void LateUpdate()
    {
        //When game is running
        if (GlobalVariables.runActive)
        {
            hasEverRun = true;

            if (!locomotor.activeSelf)
                locomotor.SetActive(true);

            isResetting = false;
            return;
        }

        //Block fade on first spawn
        if (!hasEverRun)
        {
            locomotor.SetActive(false);
            cameraRig.position = resetPoint.position;

            Vector3 currentRot = trackingSpace.eulerAngles;
            trackingSpace.rotation = Quaternion.Euler(
                currentRot.x,
                targetYaw,
                currentRot.z
            );
            GlobalVariables.hasTeleported = true;
            return;
        }

        //Start fade
        if (!isResetting)
        {
            GlobalVariables.hasTeleported = false;
            isResetting = true;

            locomotor.SetActive(false);
            fadeStartTime = Time.time;
            fade.FadeOut();
        }

        if (!GlobalVariables.hasTeleported && Time.time - fadeStartTime >= fade.fadeTime)
        {
            cameraRig.position = resetPoint.position;

            Vector3 currentRot = trackingSpace.eulerAngles;
            trackingSpace.rotation = Quaternion.Euler(
                currentRot.x,
                targetYaw,
                currentRot.z
            );

            fade.FadeIn();
            GlobalVariables.tackled = false;
            GlobalVariables.hasTeleported = true;
            GlobalVariables.runEnd = false;
        }
    }
}
