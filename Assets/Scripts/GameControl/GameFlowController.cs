using UnityEngine;
using System.Collections;

public enum GameState
{
    StartGame,
    WaitingForSnap,
    PlayRunning,
    Resetting,
    GameOver
}

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance;

    public GameState State { get; private set; }

    public bool IsResolvingPlay { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GlobalVariables.score = 0;
        GlobalVariables.downs = 1;
        GlobalVariables.ballThrown = false;
        BeginGame();
    }

    private void BeginGame()
    {
        State = GameState.StartGame;
    }

    public void EnterWaitingForSnap()
    {
        State = GameState.WaitingForSnap;
        GlobalVariables.ballThrown = false;
    }

    public void StartPlay()
    {
        if (State != GameState.WaitingForSnap) return;

        State = GameState.PlayRunning;
    }

    public void EndPlay()
    {
        if (State != GameState.PlayRunning) return;

        State = GameState.Resetting;

        CleanupEverything();
    }

    public void OnPlayerTackled()
    {
        if (State != GameState.PlayRunning || IsResolvingPlay)
            return;

        IsResolvingPlay = true;
        State = GameState.Resetting;

        StartCoroutine(TackleSequence());
    }

    public void FinishReset()
    {
        EnterWaitingForSnap();
    }

    public void EndGame()
    {
        if (GlobalVariables.score > GlobalVariables.highscore)
            GlobalVariables.highscore = GlobalVariables.score;
        SetHighScore();
        State = GameState.GameOver;
    }

    private IEnumerator TackleSequence()
    {
        yield return new WaitForSecondsRealtime(0.4f);

        CleanupEverything();
        IsResolvingPlay = false;

    }

    private void SetHighScore()
    {
        GlobalVariables.highscore = GlobalVariables.score;
        PlayerPrefs.SetInt("HIGH_SCORE", GlobalVariables.highscore);
        PlayerPrefs.Save(); // Forces disk write
    }

    private void CleanupEverything()
    {
        DestroyByTag("Ball");
        DestroyByTag("FootballPlayer");
        DestroyByTag("Opponent");
        DestroyByTag("Rusher");
    }

    private void DestroyByTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject go in objs)
            Destroy(go);
    }
}
