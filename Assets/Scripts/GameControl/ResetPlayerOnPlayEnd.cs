using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Locomotion;

public class ResetPlayerOnPlayEnd : MonoBehaviour
{
    public Transform cameraRig;
    public Transform resetPoint;

    public OVRScreenFade fade;

    private bool hasEverRun;
    private bool isResetting;
    private float fadeStartTime;

    void Start()
    {
        fade = FindObjectOfType<OVRScreenFade>();
    }

    void LateUpdate()
    {
        GameState state = GameFlowController.Instance.State;
        if (state == GameState.StartGame)
        {
            HandleStart();
        }
        if (state == GameState.WaitingForSnap)
        {
            HandleWaiting();
        }
        else if (state == GameState.Resetting)
        {
            HandleResetting();
        }
    }

    void HandleStart()
    {
        cameraRig.position = resetPoint.position;
        return;
    }
    void HandleWaiting()
    {
        isResetting = false;
    }
    void HandleResetting()
    {
        if (!isResetting)
        {
            isResetting = true;
            fadeStartTime = Time.time;
            fade.FadeOut();
            return;
        }

        if (Time.time - fadeStartTime >= fade.fadeTime)
        {
            cameraRig.position = resetPoint.position;
            fade.FadeIn();
            isResetting = false;
            GameFlowController.Instance.FinishReset();
        }
    }
}
