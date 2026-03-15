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

    [SerializeField] private ResetPlayerOnPlayEnd resetPlayerOnPlayEnd;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GlobalVariables.score = 0;
        GlobalVariables.downs = 1;
        GlobalVariables.ballThrown = false;
        GlobalVariables.successfulPasses = 0;
        BeginGame();
    }

    private void BeginGame()
    {
        State = GameState.StartGame;
    }

    public void EnterWaitingForSnap()
    {
        State = GameState.WaitingForSnap;
    }

    public void StartPlay()
    {
        if (State != GameState.WaitingForSnap) return;

        State = GameState.PlayRunning;
    }

    public void EndPlay()
    {
        if (State != GameState.PlayRunning) return;
        DestroyByTag("Ball");
        StartCoroutine(FinishSequence());
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
        GlobalVariables.ballThrown = false;
        GlobalVariables.teammateCaught = false;
        GlobalVariables.miss = false;
        EnterWaitingForSnap();
    }

    public void EndGame()
    {
        if (GlobalVariables.score > GlobalVariables.highscore)
            SetHighScore();
        State = GameState.GameOver;
    }

    private IEnumerator TackleSequence()
    {
        yield return new WaitForSecondsRealtime(0.4f);

        CleanupPlayers();
        IsResolvingPlay = false;

    }

    private IEnumerator FinishSequence()
    {
        yield return new WaitForSecondsRealtime(2f);
        State = GameState.Resetting;
        yield return new WaitForSecondsRealtime(resetPlayerOnPlayEnd.fade.fadeTime);
        CleanupPlayers();
    }

    private void SetHighScore()
    {
        GlobalVariables.highscore = GlobalVariables.score;
        PlayerPrefs.SetInt("HIGH_SCORE", GlobalVariables.highscore);
        PlayerPrefs.Save();
    }

    private void CleanupPlayers()
    {
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
