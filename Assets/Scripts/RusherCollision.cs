using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherCollision : MonoBehaviour
{
    public AudioSource audioSource;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Opponent"))
        {
            Debug.Log("Collision Occured");
            audioSource.Play();
            GlobalVariables.lives -= 1;
            GameFlowController.Instance.EndPlay();
        }
    }
}
