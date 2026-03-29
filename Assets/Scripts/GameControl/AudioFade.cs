using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    public AudioSource Cheer;
    public AudioSource Boo;
    public void FadeOut(int audioSourceNum, float duration)
    {
        if (audioSourceNum == 0)
        {
            Cheer.Stop();
            Cheer.volume = .85f;

            Cheer.Play();
            StartCoroutine(FadeOutCoroutine(Cheer, duration));
        }
        else
        {
            Boo.Stop();
            Boo.volume = 1f;

            Boo.Play();
            StartCoroutine(FadeOutCoroutine(Boo, duration));
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
