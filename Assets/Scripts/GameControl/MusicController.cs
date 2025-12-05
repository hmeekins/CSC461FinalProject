using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    private bool musicStarted = false;
    void Update()
    {
        if (GameFlowController.Instance.State != GameState.StartGame && !musicStarted)
        {
            musicStarted = true;
            audioSource.Play();
        }
    }
}
