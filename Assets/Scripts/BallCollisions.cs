using System.Collections;
using UnityEngine;

public class BallCollisions : MonoBehaviour
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Stadium"))
        {
            GlobalVariables.lives -= 1;
            GameFlowController.Instance.EndPlay();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FootballPlayer")) 
        {
            GlobalVariables.score += 100;
            // Play catch sound
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            // Fade stadium sound
            audioFade.FadeOut(4f);
            GameFlowController.Instance.EndPlay();
        }
        else if (other.CompareTag("Opponent"))
        {
            GlobalVariables.lives -= 1;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            GameFlowController.Instance.EndPlay();
        }
    }
}
