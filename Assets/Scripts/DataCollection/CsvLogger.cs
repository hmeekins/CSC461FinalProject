using System.IO;
using System.Text;
using UnityEngine;

public static class CsvLogger
{
    public static void SaveRoundData()
    {
        string directory = Path.Combine(Application.dataPath, "csv");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string filePath = Path.Combine(directory, $"game_data{GameFlowController.Instance.UserId}.csv");
        bool fileExists = File.Exists(filePath);
        StringBuilder sb = new StringBuilder();

        if (!fileExists)
        {
            sb.AppendLine("Date,SessionNum,MovementFile,Variation,NumPasses,CompletedPasses,Accuracy,RoundDuration,AverageDistance");
        }
        string movementFile = $"movementData_{GameFlowController.Instance.UserId}{GameData.SessionNumber}.csv";
        sb.AppendLine(string.Join(",",
            System.DateTime.Now.ToString("yyyy-MM-dd"),
            GameData.SessionNumber,
            movementFile,
            GameData.Variation,
            GameData.NumPasses,
            GameData.CompletedPasses,
            GameData.Accuracy,
            GameData.RoundDuration,
            GameData.AverageDistance
        ));

        File.AppendAllText(filePath, sb.ToString());
        Debug.Log("CSV saved to: " + filePath);
    }

    public static void SaveMovementData()
    {
        string directory = Path.Combine(Application.dataPath, "csv");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string filePath = Path.Combine(directory, $"movementData_{GameFlowController.Instance.UserId}{GameData.SessionNumber}.csv");
    
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("SessionNumber,PlayNum,Timestamp,HeadPosX,HeadPosY,HeadPosZ,HeadRotX,HeadRotY,HeadRotZ,HeadRotW,LeftHandPosX,LeftHandPosY,LeftHandPosZ,LeftHandRotX,LeftHandRotY,LeftHandRotZ,LeftHandRotW,RightHandPosX,RightHandPosY,RightHandPosZ,RightHandRotX,RightHandRotY,RightHandRotZ,RightHandRotW");

        foreach (MovementSample sample in GameData.MovementSamples)
        {
            sb.AppendLine(string.Join(",",
                GameData.SessionNumber,
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
}
    
}