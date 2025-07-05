using Engine;

namespace GameApp{
    public static class GameLoop{

        public enum LoopState{
            LapStop,
            LapStart,
            CheckPoint,
        };
        private static bool isLoopInit = false;

        private static LoopState _currentState = LoopState.LapStart;
        public static LoopState CurrentState => _currentState;

        public static string _trackName = "none";
        private static string _carName = "none";

        private static Camera _camera;
        private static Car _car;
        public static Car CarInstance => _car;

        private static bool _timerRunning;
        private static ulong _startTime= 0;
        private static ulong _currentTime = 0;

        public static int maxLaps = 1;
        public static int lapCount = 1;

        private static GameObject _map;

        public static void ChangeState(LoopState newState){
            _currentState = newState;
        }

        public static bool IsState(LoopState state){
            return _currentState == state;
        }

        public static void InitGameLoop(string car, string track){
            if(isLoopInit == true){
               ObjectManager.DeleteGameObject(_carName);
               ObjectManager.DeleteGameObject(_trackName);
               
            }
            else{
                _camera = new Camera();
            }
            isLoopInit = true;

            _carName = car;
            _trackName = track;

            GameObject _map = new GameObject(track, track);
            _map.scale = 40f;
            ObjectManager.AddGameObject(_map);

            GameObject test = new GameObject(car, car);
            test.scale = 1f;
            ObjectManager.AddGameObject(test);

            _car = new Car(car, _camera);
        }

        public static void UpdateGame(){
            //Here the car.Drive() and other game logik like start, finish of the timer will done
            //also things like update the UI
            _car.Drive();
        }

        public static void HandleLapping(int id)
        {
            if (id == 102 && IsState(LoopState.CheckPoint) && lapCount <= maxLaps)
            {
                lapCount++;
                ChangeState(LoopState.LapStart);
            } else if (id == 127) {
                ChangeState(LoopState.CheckPoint);
            }

            if (lapCount > maxLaps)
            {
                GameStateManager.ChangeState(GameStateManager.GameState.Finished);
            }
        }
    }
}
