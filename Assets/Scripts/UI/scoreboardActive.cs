using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreboardActive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (GameFlowController.Instance.State == GameState.GameOver)
            gameObject.active = false;
       else
            gameObject.active = true;
    }
}
