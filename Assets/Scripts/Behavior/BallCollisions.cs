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

        if (stadiumObject != null)
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
            ShowFootball(other);

            locked = true;
            distance = Vector3.Distance(GlobalVariables.ballPosition, transform.position);
            GlobalVariables.score += Mathf.RoundToInt(distance * scoreMultiplier);
            GlobalVariables.successfulPasses += 1;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);

            if (audioFade != null)
                audioFade.FadeOut(4f);

            GameFlowController.Instance.EndPlay();
        }
        else if (other.CompareTag("Opponent"))
        {
            ShowFootball(other);

            locked = true;
            GlobalVariables.downs = 5;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            GameFlowController.Instance.EndPlay();
        }
        else if (GlobalVariables.ballThrown && other.CompareTag("Rusher"))
        {
            ShowFootball(other);

            locked = true;
            GlobalVariables.downs = 5;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            GameFlowController.Instance.EndPlay();
        }
    }

    private void ShowFootball(Collider other)
    {
        Transform football = other.transform.Find("Visuals/Football");
        football.gameObject.SetActive(true);
    }
}
