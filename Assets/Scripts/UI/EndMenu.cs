using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public GameObject locomotor;
    public GameObject canvas;
    void Start()
    {
        canvas.SetActive(false);
    }


    void Update()
    {
        if (GlobalVariables.downs > 4)
            GameFlowController.Instance.EndGame();
        if (GameFlowController.Instance.State == GameState.GameOver)
        {   
            canvas.SetActive(true);
            locomotor.SetActive(false);      
        }
        else
        {
            canvas.SetActive(false);
        }
    }
}
