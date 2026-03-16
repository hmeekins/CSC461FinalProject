using System.Collections;
using UnityEngine;

public class BallCollisions : MonoBehaviour
{
    public AudioClip catchClip;
    public int scoreMultiplier;

    private AudioFade audioFade;
    public bool locked = false;
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
            GlobalVariables.miss = true;
            GlobalVariables.downs += 1;
            audioFade.FadeOut(1, 4f);
            GameFlowController.Instance.OnIncomplete();
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
            GlobalVariables.teammateCaught = true;
            distance = Vector3.Distance(GlobalVariables.ballPosition, transform.position);
            GlobalVariables.score += Mathf.RoundToInt(distance * scoreMultiplier);
            GlobalVariables.successfulPasses += 1;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);

            audioFade.FadeOut(0, 4f);

            GameFlowController.Instance.EndPlay();
        }
        else if (other.CompareTag("Opponent"))
        {
            ShowFootball(other);

            locked = true;
            GlobalVariables.miss = true;
            GlobalVariables.downs = 5;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            audioFade.FadeOut(1, 4f);
            GameFlowController.Instance.EndPlay();
        }
        else if (GlobalVariables.ballThrown && other.CompareTag("Rusher"))
        {
            ShowFootball(other);

            locked = true;
            GlobalVariables.miss = true;
            GlobalVariables.downs = 5;
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            audioFade.FadeOut(1, 4f);
            GameFlowController.Instance.EndPlay();
        }
    }

    private void ShowFootball(Collider other)
    {
        Transform football = other.transform.Find("Visuals/Football");
        football.gameObject.SetActive(true);
    }
}
