using UnityEngine;
using System.Collections;

public enum GameState
{
    WaitingForSnap,
    PlayRunning,
    Resetting
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
        EndPlay();
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

    private IEnumerator TackleSequence()
    {
        yield return new WaitForSecondsRealtime(0.4f);

        CleanupEverything();
        IsResolvingPlay = false;

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
