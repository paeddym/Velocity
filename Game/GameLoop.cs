using Engine;

namespace GameApp{
    public static class GameLoop{
        private static string _track = "none";
        private static Car _car;
        private static string _carTexture = "none";

        private static bool _timerRunning;
        private static ulong _startTime= 0;
        private static ulong _currentTime = 0;
        
        public static void UpdateCarTexture(){
            //This is used to update the car model / texture
        }

        public static void InitGameLoop(){
            //Here each time a new "Game" gets startet by the player, the track and car is loaded
            //also things like reseting timer and object positions will be done here.
        }

        public static void UpdateGame(){
            //Here the car.Drive() and other game logik like start, finish of the timer will done
            //also things like update the UI
        }
    }
}
