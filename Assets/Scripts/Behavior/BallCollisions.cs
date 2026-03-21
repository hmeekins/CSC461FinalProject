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
            distance = Vector3.Distance(GlobalVariables.ballPosition, transform.position);

            GlobalVariables.miss = true;
            GlobalVariables.downs += 1;

            GameData.AddDistance(distance);

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
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            audioFade.FadeOut(0, 4f);

            locked = true;
            distance = Vector3.Distance(GlobalVariables.ballPosition, transform.position);
            
            ParticleSystem particles = other.GetComponent<ParticleSystem>();
            particles.Play();

            GlobalVariables.teammateCaught = true;
            GlobalVariables.score += Mathf.RoundToInt(distance * scoreMultiplier);
            GlobalVariables.successfulPasses += 1;

            GameData.RegisterCompletedPass();
            GameData.AddDistance(distance);
            
            GameFlowController.Instance.EndPlay();
        }
        else if (other.CompareTag("Opponent") || (GlobalVariables.ballThrown && other.CompareTag("Rusher")))
        {
            ShowFootball(other);
            AudioSource.PlayClipAtPoint(catchClip, other.transform.position);
            audioFade.FadeOut(1, 4f);

            locked = true;
            distance = Vector3.Distance(GlobalVariables.ballPosition, transform.position);

            GlobalVariables.miss = true;
            GlobalVariables.downs = 5;
           
            GameData.AddDistance(distance);

            GameFlowController.Instance.EndPlay();
        }
    }

    private void ShowFootball(Collider other)
    {
        Transform football = other.transform.Find("Visuals/Football");
        football.gameObject.SetActive(true);
    }
}
