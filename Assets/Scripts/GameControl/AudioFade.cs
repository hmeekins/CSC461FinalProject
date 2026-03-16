using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    public AudioSource cheer;
    public AudioSource boo;
    public void FadeOut(int audioSourceNum, float duration)
    {
        if (audioSourceNum == 0)
        {
            cheer.Stop();
            cheer.volume = .85f;

            cheer.Play();
            StartCoroutine(FadeOutCoroutine(cheer, duration));
        }
        else
        {
            boo.Stop();
            boo.volume = 1f;

            boo.Play();
            StartCoroutine(FadeOutCoroutine(boo, duration));
        }
    }

    IEnumerator FadeOutCoroutine(AudioSource audio, float duration)
    {
        float startVolume = audio.volume;

        float time = 0;
        while (time < duration)
        {
            audio.volume = Mathf.Lerp(startVolume, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audio.volume = 0;
        audio.Stop();
    }
}
