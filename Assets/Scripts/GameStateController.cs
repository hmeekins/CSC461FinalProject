using UnityEngine;

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

    public void FinishReset()
    {
        EnterWaitingForSnap();
    }

    private void CleanupEverything()
    {
        DestroyByTag("Ball");
        DestroyByTag("FootballPlayer");
        DestroyByTag("Opponent");
    }

    private void DestroyByTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject go in objs)
            Destroy(go);
    }
}
