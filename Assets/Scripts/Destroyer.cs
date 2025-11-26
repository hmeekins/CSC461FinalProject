using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    AudioSource catchSound;

    private void OnCollisionEnter(Collision other) {
        Debug.Log($"Ball hit: {other.gameObject.name} | tag: {other.gameObject.tag} | layer: {other.gameObject.layer}");
        if (other.gameObject.CompareTag("Stadium")) {
            GlobalVariables.lives -= 1;
            DestroyPlayersOnField();
            StartCoroutine(Miss(0.85f));
            Destroy(gameObject);
        }
    }

    IEnumerator Miss(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("FootballPlayer")) {
            GlobalVariables.score += 100;
            catchSound = other.GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(catchSound.clip, other.transform.position);
            DestroyPlayersOnField();
            Destroy(gameObject);
        }
    }

    private void DestroyPlayersOnField()
    {
        GameObject[] gameObjectsToDestroy = GameObject.FindGameObjectsWithTag("FootballPlayer");
        foreach (GameObject obj in gameObjectsToDestroy)
        {
            Destroy(obj);
        }
    }

}
