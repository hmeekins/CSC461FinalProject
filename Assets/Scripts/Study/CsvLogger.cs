using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class CsvLogger
{
    private static string _directory;
    private static DateTimeOffset _trialStartTime;

    public static void InitializeLogger()
    {
        string userFolder = $"User_{GameFlowController.Instance.UserId}";

        _directory = Path.Combine(
            Application.dataPath,
            "csv",
            userFolder
        );

        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
        }

        _trialStartTime = DateTimeOffset.UtcNow;

        Debug.Log("CSV logger initialized at: " + _directory);
    }

    public static void SaveRoundData(string movementFile)
    {
        string filePath = Path.Combine(
            _directory,
            $"game_data_condition_{GameData.Variation}.csv"
        );

        bool fileExists = File.Exists(filePath);
        StringBuilder sb = new StringBuilder();

        if (!fileExists)
        {
            sb.AppendLine("Date,MovementFile,Condition,NumPasses,CompletedPasses,Accuracy,NumInterceptions,NumTackles,RoundDuration(s),AverageDistance(m)");
        }

        sb.AppendLine(string.Join(",",
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            EscapeCsv(movementFile),
            GameData.Variation,
            GameData.NumPasses,
            GameData.CompletedPasses,
            GameData.Accuracy,
            GameData.NumInterceptions,
            GameData.NumTackles,
            GameData.RoundDuration,
            GameData.AverageDistance
        ));

        File.AppendAllText(filePath, sb.ToString());
        Debug.Log("Round CSV saved to: " + filePath);
    }

    public static string SaveMovementData()
    {
        string movementFileName = $"movement_data_condition_{GameData.Variation}.csv";
        string filePath = Path.Combine(_directory, movementFileName);

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("MovementFile,PlayNum,Timestamp,HeadPosX,HeadPosY,HeadPosZ,HeadRotX,HeadRotY,HeadRotZ,HeadRotW,LeftHandPosX,LeftHandPosY,LeftHandPosZ,LeftHandRotX,LeftHandRotY,LeftHandRotZ,LeftHandRotW,RightHandPosX,RightHandPosY,RightHandPosZ,RightHandRotX,RightHandRotY,RightHandRotZ,RightHandRotW");

        foreach (MovementSample sample in GameData.MovementSamples)
        {
            sb.AppendLine(string.Join(",",
                EscapeCsv(movementFileName),
                sample.PlayNum,
                sample.Timestamp,

                sample.HeadPosition.x,
                sample.HeadPosition.y,
                sample.HeadPosition.z,
                sample.HeadRotation.x,
                sample.HeadRotation.y,
                sample.HeadRotation.z,
                sample.HeadRotation.w,

                sample.LeftHandPosition.x,
                sample.LeftHandPosition.y,
                sample.LeftHandPosition.z,
                sample.LeftHandRotation.x,
                sample.LeftHandRotation.y,
                sample.LeftHandRotation.z,
                sample.LeftHandRotation.w,

                sample.RightHandPosition.x,
                sample.RightHandPosition.y,
                sample.RightHandPosition.z,
                sample.RightHandRotation.x,
                sample.RightHandRotation.y,
                sample.RightHandRotation.z,
                sample.RightHandRotation.w
            ));
        }

        File.WriteAllText(filePath, sb.ToString());
        Debug.Log("Movement CSV saved to: " + filePath);

        return movementFileName;
    }

    public static void SaveEventData(string gameEvent, int playNum, string eventDetail = "")
    {
        string filePath = Path.Combine(
            _directory,
            $"event_data_condition_{GameData.Variation}.csv"
        );

        bool fileExists = File.Exists(filePath);
        StringBuilder sb = new StringBuilder();

        if (!fileExists)
        {
            sb.AppendLine("DateTimeUTC,UnixTimeMs,TrialTimeS,Condition,PlayNum,EventType");
        }

        DateTimeOffset now = DateTimeOffset.UtcNow;

        long unixTimeMs = now.ToUnixTimeMilliseconds();
        double trialTimeS = (now - _trialStartTime).TotalSeconds;

        sb.AppendLine(string.Join(",",
            now.ToString("O"),
            unixTimeMs,
            trialTimeS.ToString("F3"),
            GameData.Variation,
            playNum,
            EscapeCsv(gameEvent)
        ));

        File.AppendAllText(filePath, sb.ToString());
        Debug.Log("Event CSV saved to: " + filePath);
    }

    private static string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }

        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        return value;
    }
}