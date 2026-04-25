using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void StartScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
