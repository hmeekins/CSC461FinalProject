using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActive : MonoBehaviour
{
    public GameObject canvas;
    void Start()
    {
        canvas.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
    }
    void OnTriggerExit(Collider other) 
    {
        canvas.SetActive(false);
    }
}
