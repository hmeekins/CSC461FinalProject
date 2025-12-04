using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using TMPro;

public class Countdown : MonoBehaviour
{
    public TMP_Text countdownText;
    public TMP_Text startText;

    public AudioSource countdownBuzzer;
    public AudioSource startBuzzer;
    private ParticleSystem particleSystem;
    private bool countdownStarted = false;

    void Start()
    {
        particleSystem = GetComponentInParent<ParticleSystem>();
        countdownText.text = "3";
        startText.text = "";
    }
    void Update()
    {
        if(GameFlowController.Instance.State == GameState.StartGame && !countdownStarted)
            StartCoroutine(countdown());
            countdownStarted = true;
    }

    private IEnumerator countdown()
    {
        yield return new WaitForSeconds(1);
        countdownBuzzer.Play();
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        countdownBuzzer.Play();
        yield return new WaitForSeconds(1);
        countdownBuzzer.Play();
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        startBuzzer.Play();
        countdownText.text = "";
        OVRInput.SetControllerVibration(.5f, .5f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(.5f, .5f, OVRInput.Controller.LTouch);
        particleSystem.Play();
        startText.text = "Start!";
        yield return new WaitForSeconds(1);
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
        particleSystem.Stop();
        startText.text = "";
        GameFlowController.Instance.EnterWaitingForSnap();
    }
}
