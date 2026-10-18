using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneLandingTrigger : MonoBehaviour
{
    public int ZoneID;
    private bool _counted;

    private void OnTriggerEnter(Collider other)
    {

        if (_counted)
            return;

        if (!other.CompareTag("Ball"))
            return;

        _counted = true;

        TutorialController.Instance.ZoneCompleted(ZoneID);
    }
}