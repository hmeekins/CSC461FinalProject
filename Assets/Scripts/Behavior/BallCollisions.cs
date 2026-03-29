using System.Collections;
using UnityEngine;

public class BallCollisions : MonoBehaviour
{
    [SerializeField] private AudioClip _catchClip;
    [SerializeField] private int _scoreMultiplier;

    private AudioFade _audioFade;
    private bool _locked = false;
    private float _distance;

    void Start()
    {
        GameObject stadiumObject = GameObject.FindGameObjectWithTag("Stadium");

        _audioFade = stadiumObject.GetComponentInParent<AudioFade>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_locked)
            return;

        if (other.gameObject.CompareTag("Stadium"))
        {
            _locked = true;
            _distance = Vector3.Distance(GlobalVariables.ballPosition, transform.position);

            GlobalVariables.miss = true;
            GlobalVariables.downs += 1;

            GameData.AddDistance(_distance);

            GameFlowController.Instance.OnIncomplete();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_locked)
            return;

        if (other.CompareTag("FootballPlayer"))
        {
            ShowFootball(other);
            AudioSource.PlayClipAtPoint(_catchClip, other.transform.position);
            _audioFade.FadeOut(0, 4f);

            _locked = true;
            _distance = Vector3.Distance(GlobalVariables.ballPosition, transform.position);
            
            ParticleSystem particles = other.GetComponent<ParticleSystem>();
            particles.Play();

            GlobalVariables.teammateCaught = true;
            GlobalVariables.score += Mathf.RoundToInt(_distance * _scoreMultiplier);
            GlobalVariables.successfulPasses += 1;

            GameData.RegisterCompletedPass();
            GameData.AddDistance(_distance);
            
            GameFlowController.Instance.EndPlay();
        }
        else if (other.CompareTag("Opponent") || (GlobalVariables.ballThrown && other.CompareTag("Rusher")))
        {
            ShowFootball(other);
            AudioSource.PlayClipAtPoint(_catchClip, other.transform.position);
            _audioFade.FadeOut(1, 4f);

            _locked = true;
            _distance = Vector3.Distance(GlobalVariables.ballPosition, transform.position);

            GlobalVariables.miss = true;
            GlobalVariables.downs = 5;
           
            GameData.AddDistance(_distance);

            GameFlowController.Instance.EndPlay();
        }
    }

    private void ShowFootball(Collider other)
    {
        Transform football = other.transform.Find("Visuals/Football");
        football.gameObject.SetActive(true);
    }
}
