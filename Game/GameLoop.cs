using Engine;
using OpenTK.Windowing.Common;
using System.Drawing;
using OpenTK.Mathematics;

namespace GameApp{
    public static class GameLoop{

        public enum LoopState{
            CountDown,
            LapStart,
            CheckPoint,
        };
        private static bool isLoopInit = false;

        private static LoopState _currentState = LoopState.CountDown;
        public static LoopState CurrentState => _currentState;

        public static string _trackName = "none";
        private static string _carName = "none";

        private static Camera _camera;
        public static Camera CameraInstance => _camera;
        private static Car _car;
        public static Car CarInstance => _car;

        private static double _totalElapsedTime = 0f;
        private static double _startTime = 0f;
        private static double _currentTime = 0f;
        public static double CurrentTime => _currentTime;

        private static double _currentLapTime = 0f;
        private static double _previousLapStart = 0f;
        private static double _bestLapTime = double.MaxValue;
        public static double BestLapTime => _bestLapTime;
        private static double _trackRecord = double.MaxValue;
        public static double TrackRecord => _trackRecord;

        private static double _previousSplitToCheckPoint = 0f;
        private static double _currentSplitToCheckpoint = 0f;
        private static double _previousSplitToStart = 0f;
        private static double _currentSplitToStart = 0f;
        private static double _previousSplitToStartStart = 0f;  // At which point in time the checkpoint was reached last
        private static double _splitDifference = 0f;
        public static double SplitDifference => _splitDifference;
        public static bool ShowSplits = false;
        private static double _showSplitsDuration = 3f;
        public static double ShowSplitsTimer = _showSplitsDuration;  // Show split information for 3 seconds after crossing a checkpoint

        private static int _maxLaps = 3;
        public static int MaxLaps => _maxLaps;

        private static int _lapCount = 1;
        public static int LapCount => _lapCount;


        private static GameObject _map;

        private static float _countdownTime = 3f;
        public static float CountdownTime => _countdownTime;
        private static bool _countdownStarted = false;

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
            _map.scale = 0f;
            _map.scaleVector = new Vector3(60f, 40f, 0f);
            ObjectManager.AddGameObject(_map);

            GameObject test = new GameObject(car, car);
            test.scale = 1f;
            ObjectManager.AddGameObject(test);

            _car = new Car(car, _camera);
            _countdownTime = 3.0f;
            _countdownStarted = false;
            _currentState = LoopState.CountDown;
            _lapCount = 1;
            _startTime = 0;
            _currentTime = 0;
            _totalElapsedTime = 0;
            _currentLapTime = 0;
            _previousLapStart = 0;
            _bestLapTime = double.MaxValue;
            _currentSplitToCheckpoint = 0f;
            _previousSplitToCheckPoint = 0f;
            _currentSplitToStart = 0f;
            _previousSplitToStart = 0f;
            _previousSplitToStartStart = 0f;
            _splitDifference = 0f;
            ShowSplits = false;
            ShowSplitsTimer = _showSplitsDuration;

            // Load best lap time from file if available
            var fileBest = LapTimeStorage.LoadBestLapTime(_trackName);
            if (fileBest.HasValue)
            {
                _trackRecord = fileBest.Value;
            } else
            {
                _trackRecord = double.MaxValue;
            }  
        }

        public static void UpdateGame(){
            //Here the car.Drive() and other game logik like start, finish of the timer will done
            //also things like update the UI
            FrameEventArgs _event = InputProvider.GetFrameEvent();
            _totalElapsedTime += _event.Time;

            if (IsState(LoopState.CountDown))
            {
                if (!_countdownStarted)
                {
                    _countdownStarted = true;
                    // So the car gets drawn properly during countdown instead of a wide angle shot of the track
                    _car.Drive();
                }
                _countdownTime -= (float)_event.Time;
                if (_countdownTime <= -1f)
                {
                    _countdownTime = 0f;
                    _countdownStarted = false;
                    _startTime = _totalElapsedTime;
                    _previousLapStart = _startTime;

                    ChangeState(LoopState.LapStart);
                }
                return;
            }
            _currentTime = _totalElapsedTime - _startTime;
            _currentLapTime = _totalElapsedTime - _previousLapStart;
            _car.Drive();
        }

        public static void HandleLapping(int id)
        {
            if (id == 102 && IsState(LoopState.CheckPoint) && _lapCount <= _maxLaps)
            {

                _currentSplitToStart = _totalElapsedTime - _previousSplitToStartStart;
                _splitDifference = _currentSplitToStart - _previousSplitToStart;
                _previousSplitToStart = _currentSplitToStart;
                _previousLapStart = _totalElapsedTime;
                _lapCount++;
                ShowSplitsTimer = _showSplitsDuration;

                if (_currentLapTime < _bestLapTime)
                {
                    _bestLapTime = _currentLapTime;                   
                }
                
                ChangeState(LoopState.LapStart);
            } else if (id == 127) {
                if (!IsState(LoopState.CheckPoint)){                    
                    _currentSplitToCheckpoint = _totalElapsedTime - _previousLapStart;
                    _splitDifference = _currentSplitToCheckpoint - _previousSplitToCheckPoint;
                    _previousSplitToCheckPoint = _currentSplitToCheckpoint;
                    _previousSplitToStartStart = _totalElapsedTime;
                    ShowSplitsTimer = _showSplitsDuration;
                }
                if (_lapCount > 1)
                {
                    ShowSplits = true;
                }
                ChangeState(LoopState.CheckPoint);
            }

            if (_lapCount > _maxLaps)
            {
                if(_currentTime < _trackRecord) {
                    _trackRecord = _currentTime;
                    LapTimeStorage.SaveBestLapTime(_trackName, _trackRecord);
                }
                GameStateManager.ChangeState(GameStateManager.GameState.Finished);                
            }
        }
    }
}
