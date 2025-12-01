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

    private bool hasEverRun = false;
    private bool isResetting = false;
    private float fadeStartTime;

    private void Start()
    {
        fade = FindObjectOfType<OVRScreenFade>();
    }

    private void LateUpdate()
    {
        if (GameFlowController.Instance.State == GameState.WaitingForSnap)
        {
            locomotor.SetActive(false);
            isResetting = false;
            return;
        }
        //When game is running
        if (GameFlowController.Instance.State == GameState.PlayRunning)
        {
            hasEverRun = true;

            if (!locomotor.activeSelf)
                locomotor.SetActive(true);

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
            return;
        }

        //Start fade
        if (!isResetting)
        {
            isResetting = true;

            locomotor.SetActive(false);
            fadeStartTime = Time.time;
            fade.FadeOut();
        }

        if (Time.time - fadeStartTime >= fade.fadeTime)
        {
            cameraRig.position = resetPoint.position;

            Vector3 currentRot = trackingSpace.eulerAngles;
            //trackingSpace.rotation = Quaternion.Euler(
                //currentRot.x,
                //targetYaw,
                //currentRot.z
            //);

            fade.FadeIn();
            isResetting = false;
            GameFlowController.Instance.FinishReset();
        }
    }
}
