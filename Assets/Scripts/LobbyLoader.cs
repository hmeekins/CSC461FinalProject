using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyLoader : MonoBehaviour
{
    public void LoadLobby()
    {
        SceneManager.LoadScene("LockerRoom");
    }
}
