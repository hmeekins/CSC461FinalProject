using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Stadium")) {
            StartCoroutine(Miss(0.85f));
            Global.lives -= 1;
        }
    }

    IEnumerator Miss(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Global.score += 100;
            Destroy(gameObject);
        }
    }

}
