using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Locomotion;

public class ResetPlayerOnPlayEnd : MonoBehaviour
{
    public Transform cameraRig;
    public Transform resetPoint;
    public GameObject locomotor;

    private OVRScreenFade fade;

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

        if (state == GameState.WaitingForSnap)
        {
            HandleWaiting();
        }
        else if (state == GameState.PlayRunning)
        {
            HandlePlayRunning();
        }
        else
        {
            HandleResetting();
        }
    }

    void HandleWaiting()
    {
        locomotor.SetActive(false);
        isResetting = false;
    }

    void HandlePlayRunning()
    {
        hasEverRun = true;

        if (!locomotor.activeSelf)
            locomotor.SetActive(true);
    }

    void HandleResetting()
    {
        if (GameFlowController.Instance.IsResolvingPlay)
            return;
        if (!hasEverRun)
        {
            locomotor.SetActive(false);
            cameraRig.position = resetPoint.position;
            return;
        }

        if (!isResetting)
        {
            isResetting = true;
            locomotor.SetActive(false);
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
