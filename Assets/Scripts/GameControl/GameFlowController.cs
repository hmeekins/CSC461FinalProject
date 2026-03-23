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

public enum GameVariation
{
    Variation1 = 1,
    Variation2 = 2,
    Variation3 = 3,
    Variation4 = 4,
    Variation5 = 5,
    Variation6 = 6
}


public class GameFlowController : MonoBehaviour
{
    
    public static GameFlowController Instance;
    public GameVariation Variation;
    public GameState State { get; private set; }

    public bool IsResolvingPlay { get; private set; } = false;

    [SerializeField] private ResetPlayerOnPlayEnd resetPlayerOnPlayEnd;

    [SerializeField] private RusherCollision rusherCollision;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GlobalVariables.ResetData();
        BeginGame();
    }

    private void BeginGame()
    {
        GameData.StartNewRound();
        State = GameState.StartGame;
    }

    public void EnterWaitingForSnap()
    {
        GlobalVariables.ballThrown = false;
        GlobalVariables.teammateCaught = false;
        GlobalVariables.miss = false;
        GlobalVariables.tackled = false;
        rusherCollision.hasTriggered = false;
        State = GameState.WaitingForSnap;
    }

    public void StartPlay()
    {
        if (State != GameState.WaitingForSnap) return;

        State = GameState.PlayRunning;
    }

    public void EndPlay()
    {
        if (GlobalVariables.downs > 4)
        {
            GameFlowController.Instance.EndGame();
            return;
        }

        IsResolvingPlay = true;
        DestroyByTag("Ball");
        StartCoroutine(FinishSequence());
    }

    public void OnIncomplete()
    {
        if (GlobalVariables.downs > 4)
        {
            GameFlowController.Instance.EndGame();
            return;
        }
        
        IsResolvingPlay = true;
        StartCoroutine(FinishSequence());
    }

    public void OnPlayerTackled()
    {
        if (GlobalVariables.downs > 4)
        {
            GameFlowController.Instance.EndGame();
            return;
        }

        IsResolvingPlay = true;
        DestroyByTag("Ball");
        StartCoroutine(FinishSequence());
    }

    public void FinishReset()
    {
        IsResolvingPlay = false;
        EnterWaitingForSnap();
    }

    public void EndGame()
    {
        if (GlobalVariables.score > GlobalVariables.highscore)
            SetHighScore();
        CleanupField();
        GameData.FinalizeRound();
        State = GameState.GameOver;
    }

    private IEnumerator FinishSequence()
    {
        yield return new WaitForSecondsRealtime(2f);
        State = GameState.Resetting;
        yield return new WaitForSecondsRealtime(resetPlayerOnPlayEnd.fade.fadeTime);
        CleanupField();
    }

    private void SetHighScore()
    {
        GlobalVariables.highscore = GlobalVariables.score;
        PlayerPrefs.SetInt("HIGH_SCORE", GlobalVariables.highscore);
        PlayerPrefs.Save();
    }

    private void CleanupField()
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
