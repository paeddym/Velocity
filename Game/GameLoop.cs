using Engine;

namespace GameApp{
    public static class GameLoop{

        public enum LoopState{
            LapStart,
            LapRunning,
            LapStop,
            CheckPoint,
        };
        private static bool isLoopInit = false;

        private static LoopState _currentState = LoopState.LapStart;
        public static LoopState CurrentState => _currentState;

        public static string _trackName = "none";
        private static string _carName = "none";

        private static Camera _camera;
        private static Car _car;

        private static bool _timerRunning;
        private static ulong _finishTime = 0;
        private static ulong _checkpointTime = 0;

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
            _currentState = LoopState.LapStart;

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
    }
}
