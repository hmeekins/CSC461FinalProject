using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialParticles : MonoBehaviour
{
    public void Poof(Vector3 position)
    {
        ParticleSystem poof = GetComponent<ParticleSystem>();
        poof.transform.position = position;
        poof.Play();
    }
}
