using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollision : MonoBehaviour
{
    private bool _locked = false;
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        if (_locked) 
            return;

        if (TutorialController.Instance.TutorialStep == TutorialStep.SpawnBall) {
            TutorialController.Instance.HitFirstTarget();
            _locked = true;
            StartCoroutine(Unlock());
            return;
        }
        else {
            TutorialController.Instance.HitSecondTarget();
        }
    }

    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(1);
        _locked = false;
    }
}
