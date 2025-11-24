using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    AudioSource catchSound;
    private void OnCollisionEnter(Collision other) {
        Debug.Log($"Ball hit: {other.gameObject.name} | tag: {other.gameObject.tag} | layer: {other.gameObject.layer}");
        if (other.gameObject.CompareTag("Stadium")) {
            StartCoroutine(Miss(0.85f));
            GlobalVariables.lives -= 1;
        }
    }

    IEnumerator Miss(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GlobalVariables.score += 100;
            catchSound = other.GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(catchSound.clip, other.transform.position);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
