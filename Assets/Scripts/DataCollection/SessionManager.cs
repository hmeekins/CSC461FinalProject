using UnityEngine;

public static class SessionManager
{
    private const string SessionKey = "StudySessionNumber";

    public static int GetSessionNumber()
    {
        return PlayerPrefs.GetInt(SessionKey, 1);
    }

    public static void SetSessionNumber(int sessionNumber)
    {
        PlayerPrefs.SetInt(SessionKey, sessionNumber);
        PlayerPrefs.Save();
    }

    public static void IncrementSessionNumber()
    {
        int current = GetSessionNumber();
        PlayerPrefs.SetInt(SessionKey, current + 1);
        PlayerPrefs.Save();
    }

    public static void ResetSessionNumber()
    {
        PlayerPrefs.SetInt(SessionKey, 1);
        PlayerPrefs.Save();
    }
}