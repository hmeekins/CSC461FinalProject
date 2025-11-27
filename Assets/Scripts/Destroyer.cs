using System.Collections;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public AudioClip catchClip;

    private AudioFade audioFade;

    void Start()
    {
        GameObject stadiumObject = GameObject.FindGameObjectWithTag("Stadium");
        
        audioFade = stadiumObject.GetComponentInParent<AudioFade>();
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

        // Play catch sound
        AudioSource.PlayClipAtPoint(catchClip, other.transform.position);

        // Fade stadium sound
        audioFade.FadeOut(3f);

        Destroy(gameObject);
        DestroyPlayersOnField();
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
