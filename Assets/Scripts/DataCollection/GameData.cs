using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static int Variation { get; private set; } = 0;
    public static int NumPasses { get; private set; } = 0;
    public static int CompletedPasses { get; private set; } = 0;
    public static float Accuracy { get; private set; } = 0f;
    public static int RoundDuration { get; private set; } = 0;
    public static List<float> Distances { get; private set; } = new List<float>();
    public static float AverageDistance { get; private set; } = 0f;

    public static void StartNewRound()
    {
        Variation = 0;
        NumPasses = 0;
        CompletedPasses = 0;
        Accuracy = 0f;
        RoundDuration = 0;
        AverageDistance = 0f;
        Distances.Clear();
    }

    public static void SetVariation(int variation)
    {
        Variation = variation;
    }

    public static void RegisterPass()
    {
        NumPasses++;
    }

    public static void RegisterCompletedPass()
    {
        CompletedPasses++;
    }

    public static void SetRoundDuration(int roundDuration)
    {
        RoundDuration = roundDuration;
    }

    public static void AddDistance(float distance)
    {
        Distances.Add(distance);
    }

    public static void FinalizeRound()
    {
        CalculateAccuracy();
        CalculateAverageDistance();
    }

    private static void CalculateAccuracy()
    {
        if (NumPasses == 0)
        {
            Accuracy = 0f;
            return;
        }

        Accuracy = (float)System.Math.Round((float)CompletedPasses / NumPasses, 2);
    }

    private static void CalculateAverageDistance()
    {
        if (Distances.Count == 0)
        {
            AverageDistance = 0f;
            return;
        }

        float total = 0f;

        for (int i = 0; i < Distances.Count; i++)
        {
            total += Distances[i];
        }

        AverageDistance = (float)System.Math.Round(total / Distances.Count, 2);
    }
}