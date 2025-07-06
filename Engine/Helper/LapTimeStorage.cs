using System;
using System.IO;

namespace Engine
{
    public static class LapTimeStorage
    {
        private static string GetFilePath(string trackName)
        {
            return $"laptime_{trackName}.txt";
        }

        public static double? LoadBestLapTime(string trackName)
        {
            string filePath = GetFilePath(trackName);
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                if (double.TryParse(content, out double bestTime))
                    return bestTime;
            }
            return null;
        }

        public static void SaveBestLapTime(string trackName, double lapTime)
        {
            string filePath = GetFilePath(trackName);
            File.WriteAllText(filePath, lapTime.ToString("F6"));
        }
    }
}
