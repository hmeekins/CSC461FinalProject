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
        if (GlobalVariables.runEnd || (runSpawner.rightRunner == null && runSpawner.leftRunner == null))
        {
            GlobalVariables.runActive = false;
            DestroyEverything();
            GlobalVariables.runEnd = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Stadium"))
        {
            GlobalVariables.lives -= 1;
            GlobalVariables.runEnd = true;
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
            GlobalVariables.runEnd = true;
        }
        else if (other.CompareTag("Opponent"))
        {
            GlobalVariables.lives -= 1;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            Destroy(gameObject);
            GlobalVariables.runEnd = true;
        }
    }

    private void DestroyEverything()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("FootballPlayer");
        foreach (GameObject instance in objects)
        {
            Destroy(instance);
        }
        objects = GameObject.FindGameObjectsWithTag("Opponent");
        foreach (GameObject instance in objects)
        {
            Destroy(instance);
        }
        objects = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject instance in objects)
        {
            Destroy(instance);
        }
    }
}
