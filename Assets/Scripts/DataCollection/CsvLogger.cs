using System.IO;
using System.Text;
using UnityEngine;

public static class CsvLogger
{
    private static string FilePath
    {
        get
        {
            return Path.Combine(Application.dataPath, "game_data.csv");
        }
    }

    public static void SaveRoundData()
    {
        bool fileExists = File.Exists(FilePath);
        StringBuilder sb = new StringBuilder();

        if (!fileExists)
        {
            sb.AppendLine("Date,Variation,NumPasses,CompletedPasses,Accuracy,RoundDuration,AverageDistance");
        }

        sb.AppendLine(string.Join(",",
            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            GameData.Variation,
            GameData.NumPasses,
            GameData.CompletedPasses,
            GameData.Accuracy,
            GameData.RoundDuration,
            GameData.AverageDistance
        ));

        File.AppendAllText(FilePath, sb.ToString());
        Debug.Log("CSV saved to: " + FilePath);
    }
}