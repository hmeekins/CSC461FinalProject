using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollision : MonoBehaviour
{
    private bool _locked = false;
    void OnTriggerEnter(Collider other)
    {
        if (_locked) 
            return;

        if (TutorialController.Instance.TutorialStep == TutorialStep.SpawnBall) {
            Destroy(other);
            TutorialController.Instance.HitFirstTarget();
            _locked = true;
            StartCoroutine(Unlock());
            return;
        }
        else {
            Destroy(other);
            TutorialController.Instance.HitSecondTarget();
        }
    }

    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(1);
        _locked = false;
    }
}
