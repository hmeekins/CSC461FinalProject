using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialStep
    {
        GameBasics,
        SpawnBall,
        FinishedZones,
        HitTarget,
        HitMovingTarget,
        Defenders,
        Rusher,
        TutorialOver
    }

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;
    public TutorialStep TutorialStep;
    [SerializeField] private AudioSource _chime;
    private bool[] _zoneHit = new bool[3];
    public int ZonesHit { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    void Start() 
    {
        GameFlowController.Instance.EnterWaitingForSnap();
    }

    public void StartTutorial()
    {
        Instance.TutorialStep = TutorialStep.GameBasics;
    }

    public void BallSpawned()
    {
        Instance.TutorialStep = TutorialStep.SpawnBall;
        _chime.Play();
    }
    public void ZoneCompleted(int zoneID)
    {
        if (_zoneHit[zoneID])
            return;

        _zoneHit[zoneID] = true;
        ZonesHit++;
    }

    public void FinishZones()
    {
        Instance.TutorialStep = TutorialStep.FinishedZones;
        _chime.Play();
    }

    public void HitFirstTarget()
    {
        Instance.TutorialStep = TutorialStep.HitTarget;
        _chime.Play();
    }

    public void HitSecondTarget()
    {
        Instance.TutorialStep = TutorialStep.HitMovingTarget;
        GameFlowController.Instance.BeginGame();
        _chime.Play();
    }

    public void CoverDefenders()
    {
        Instance.TutorialStep = TutorialStep.Defenders;
    }

    public void CoverRusher()
    {
        Instance.TutorialStep = TutorialStep.Rusher;
        _chime.Play();
    }

    public void EndTutorial()
    {
        Instance.TutorialStep = TutorialStep.TutorialOver;
        _chime.Play();
    }
}
