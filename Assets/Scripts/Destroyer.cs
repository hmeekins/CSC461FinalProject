using System.Collections;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public AudioClip catchClip;

    public RunSpawner runSpawner;

    private AudioFade audioFade;

    void Start()
    {
        GameObject stadiumObject = GameObject.FindGameObjectWithTag("Stadium");
        GameObject spawnerObject = GameObject.FindGameObjectWithTag("Spawner");
        
        audioFade = stadiumObject.GetComponentInParent<AudioFade>();
        runSpawner = spawnerObject.GetComponent<RunSpawner>();
    }

    void Update()
    {
        if (runSpawner.rightRunner != null)
        {
            float rightX = runSpawner.rightRunner.transform.position.x;
            if (rightX == GlobalVariables.rightTargetX)
            {
                Destroy(runSpawner.rightRunner);
            }   
        }
        if (runSpawner.leftRunner != null)
        {
            float leftX = runSpawner.leftRunner.transform.position.x;
            if (leftX == GlobalVariables.leftTargetX)
            {
                Destroy(runSpawner.leftRunner);
            }   
        }
        if (runSpawner.rightRunner == null && runSpawner.leftRunner == null)
        {
            DestroyPlayersOnField();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(
    "Hit Object: " + other.gameObject.name +
    " | Tag: " + other.gameObject.tag +
    " | Layer: " + other.gameObject.layer
);
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
        if (other.CompareTag("FootballPlayer")) 
        {
            GlobalVariables.score += 100;
            // Play catch sound
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            // Fade stadium sound
            audioFade.FadeOut(3f);
            Destroy(gameObject);
            DestroyPlayersOnField();
        }
        else if (other.CompareTag("Opponent"))
        {
            GlobalVariables.lives -= 1;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            Destroy(gameObject);
            DestroyPlayersOnField();
        }
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
