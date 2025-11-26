using System.Collections;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [Header("References")]
    public GameObject stadium;        // Stadium object with AudioFade
    public AudioClip catchClip;       // Clip for catching
    public AudioClip stadiumClip;     // Clip to play for the stadium audio

    private AudioFade audioFade;

    void Start()
    {
        audioFade = stadium.GetComponent<AudioFade>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Stadium"))
        {
            GlobalVariables.lives -= 1;
            DestroyPlayersOnField();
            StartCoroutine(Miss(0.85f));
        }
    }

    IEnumerator Miss(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("FootballPlayer"))
            return;

        GlobalVariables.score += 100;

        // Play catch sound immediately
        AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
        // Play stadium audio safely after one frame
        StartCoroutine(PlayStadiumClipWithFade());

        Destroy(gameObject);
        DestroyPlayersOnField();
    }

    private IEnumerator PlayStadiumClipWithFade()
    {
        // Create a temporary GameObject for the audio
        GameObject temp = new GameObject("TempStadiumAudio");
        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = stadiumClip;
        source.volume = 1f;
        source.spatialBlend = 0f; // 2D
        source.Play();

        // Fade out manually over 2 seconds
        float duration = 2f;
        float time = 0f;
        float startVolume = source.volume;

        while (time < duration)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        source.Stop();
        Destroy(temp);
    }


    private void DestroyPlayersOnField()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("FootballPlayer");
        foreach (GameObject player in players)
        {
            Destroy(player);
        }
    }
}
