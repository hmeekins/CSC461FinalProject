using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollision : MonoBehaviour
{
    private bool _locked = false;
    private TutorialParticles _particles;

    void Start()
    {
        _particles = GameObject.Find("Particles").GetComponent<TutorialParticles>();
    }
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        if (_locked) 
            return;

        if (TutorialController.Instance.TutorialStep == TutorialStep.FinishedZones) {
            TutorialController.Instance.HitFirstTarget();
            _particles.Poof(transform.position);
            _locked = true;
            StartCoroutine(Unlock());
            return;
        }
        else 
        {
            TutorialController.Instance.HitSecondTarget();
            _particles.Poof(transform.position);
        }
    }

    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(1);
        _locked = false;
    }
}
