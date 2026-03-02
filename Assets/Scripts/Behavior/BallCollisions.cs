using System.Collections;
using UnityEngine;

public class BallCollisions : MonoBehaviour
{
    public AudioClip catchClip;
    public int scoreMultiplier;

    private AudioFade audioFade;
    private bool locked = false;
    private float distance;


    void Start()
    {
        GameObject stadiumObject = GameObject.FindGameObjectWithTag("Stadium");
        
        audioFade = stadiumObject.GetComponentInParent<AudioFade>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (locked)
            return;
        if (other.gameObject.CompareTag("Stadium"))
        {
            locked = true;
            GlobalVariables.downs += 1;
            GameFlowController.Instance.EndPlay();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (locked)
            return;
        if (other.CompareTag("FootballPlayer")) 
        {
            locked = true;
            distance = Vector3.Distance(GlobalVariables.ballPosition, gameObject.transform.position);
            GlobalVariables.score += Mathf.RoundToInt(distance * scoreMultiplier);
            GlobalVariables.successfulPasses += 1;
            // Play catch sound
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            // Fade stadium sound
            audioFade.FadeOut(4f);
            GameFlowController.Instance.EndPlay();
        }
        else if (other.CompareTag("Opponent"))
        {
            locked = true;
            GlobalVariables.downs = 5;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            GameFlowController.Instance.EndPlay();
        }
        //Only registers collision if ball is thrown to prevent conflicts when rusher collides with player
        else if (GlobalVariables.ballThrown)
        {
            if (other.CompareTag("Rusher"))
            {
                locked = true;
                GlobalVariables.downs = 5;
                AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
                GameFlowController.Instance.EndPlay();
            }
        }
    }
}
