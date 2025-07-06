using System.Diagnostics;

public static class GameTimer{
    private static Stopwatch stopwatch = new Stopwatch();

    public static void Start() {
        if (!stopwatch.IsRunning){
            stopwatch.Start();
        }
    }

    public static void Pause() {
        if (stopwatch.IsRunning){
            stopwatch.Stop();
        }
    }

    public static void Resume() {
        if (!stopwatch.IsRunning){
            stopwatch.Start();
        }
    }

    public static void Reset() {
        stopwatch.Reset();
    }

    public static string GetFormattedTime() {
        TimeSpan ts = stopwatch.Elapsed;
        return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
    }

    public static long GetElapsedMilliseconds(){
            return stopwatch.ElapsedMilliseconds;
    }
}

