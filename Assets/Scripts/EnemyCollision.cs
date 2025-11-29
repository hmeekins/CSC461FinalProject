using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GlobalVariables.lives -= 1;
            Destroy(gameObject);
            GlobalVariables.runActive = false;
        }
    }
}
