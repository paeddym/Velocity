using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Engine;

namespace GameApp{
    public class Car {

        private float _speed = 0f;
        private string _name;
        private Camera _camera;
        private GameObject _car;
        private bool _trackTwoStart = false;

        Vector2[] localOffsets;
        List<Vector2> hitboxPoints;

        private bool _collisionAnimActive = false;
        private double _collisionAnimStartTime = 0;
        private Vector2 _collisionAnimPosition = Vector2.Zero;
        private const double _collisionAnimDuration = 0.5;

        public bool CollisionAnimActive => _collisionAnimActive;
        public double CollisionAnimStartTime => _collisionAnimStartTime;
        public Vector2 CollisionAnimPosition => _collisionAnimPosition;
        public double CollisionAnimDuration => _collisionAnimDuration;

        public Car(string name, Camera camera) {
            this._name = name;
            this._camera = camera;
            this._car = ObjectManager.GetGameObject(this._name);
            this._car.objectPos.X = -20.5f;
            this._car.objectPos.Y = 0.3f;
        }

        public void Drive() {
            if (GameLoop._trackName == "track2" && _trackTwoStart == false){
                Console.WriteLine($"Set car pos for track2: {GameLoop._trackName}");
                _car.objectPos.X = -18f;
                _trackTwoStart = true;
            }
            if (GameLoop._trackName != "track2") {
                _trackTwoStart = false;
            }
            FrameEventArgs _event = InputProvider.GetFrameEvent();
            float _deltaTime = (float)_event.Time;
            KeyboardState _keyboardState = InputProvider.GetKeyboardState();
            float maxSpeed = 15f * _deltaTime;
            float revMaxSpeed = -3.0f * _deltaTime;


            _car.objectPos.Z = .1f;

            float carAngle = _car.objectPos.W;
            float halfWidth = 0.3f;
            float halfLength = 0.45f;

            // Rotation matrix
            float cos = (float)Math.Cos(carAngle);
            float sin = (float)Math.Sin(carAngle);

            // Define corners of car in local space
            this.localOffsets = new Vector2[] {
                new Vector2(-halfWidth, -halfLength), // back-left
                new Vector2(halfWidth, -halfLength),  // back-right
                new Vector2(-halfWidth, halfLength),  // front-left
                new Vector2(halfWidth, halfLength),   // front-right
            };

            // Transform to world space
            this.hitboxPoints = new List<Vector2>();
            foreach (var offset in localOffsets)
            {
                float worldX = _car.objectPos.X + (offset.X * cos - offset.Y * sin);
                float worldY = _car.objectPos.Y + (offset.X * sin + offset.Y * cos);
                hitboxPoints.Add(new Vector2(worldX, worldY));
            }

            
            float acceleration = 0.0005f;     // Acceleration rate
            float deceleration = 0.000000005f;     // Reverse acceleration rate

            checkPointCollision();

            // Apply input
            if (_keyboardState.IsKeyDown(Keys.W)){
                _speed += acceleration * _deltaTime;
                ParticleManager.Emit(_car, _speed);
                if (_speed > maxSpeed) _speed = maxSpeed;
            }
            else if (_keyboardState.IsKeyDown(Keys.S)){
                _speed -= (acceleration+0.0008f)* _deltaTime;
                if (_speed < revMaxSpeed) _speed = revMaxSpeed;
            }
            else{
                // Optional: natural friction to stop when no input
                if (_speed > 0){
                    _speed -= acceleration * _deltaTime;
                    if (_speed < 0) _speed = 0;
                }
                else if (_speed < 0){
                    _speed += acceleration * _deltaTime;
                    if (_speed > 0) _speed = 0;
                }
            }

            // Adjust steering based on whether the car is moving forward or backward
            if (_keyboardState.IsKeyDown(Keys.A) && _speed != 0){
                if (_speed > 0){
                    _car.objectPos.W += 1.5f * _deltaTime;
                }
                else{
                    _car.objectPos.W -= 1.5f * _deltaTime;
                }
            }

            if (_keyboardState.IsKeyDown(Keys.D) && _speed != 0){
                if (_speed > 0){
                    _car.objectPos.W -= 1.5f * _deltaTime;
                }
                else{
                    _car.objectPos.W += 1.5f * _deltaTime;
                }
            }

            _car.front.X = (float)Math.Sin(-1 * _car.objectPos.W);
            _car.front.Y = (float)Math.Cos(_car.objectPos.W);

            //_posX = _posX + front.X * _speed;
            //_posY = _posY + front.Y * _speed;
            _car.objectPos.X = _car.objectPos.X + _car.front.X * _speed;
            _car.objectPos.Y = _car.objectPos.Y + _car.front.Y * _speed;

            // Deactivate collision animation if duration has passed
            if (_collisionAnimActive && (GameLoop.CurrentTime - _collisionAnimStartTime) > _collisionAnimDuration)
            {
                _collisionAnimActive = false;
            }

            ParticleManager.Update(_deltaTime);

            _camera.UseLockCam(_car.objectPos.X, _car.objectPos.Y, _car.objectPos.W);
        }

        public Vector2 GetSize(){
            return new Vector2(1.0f, 1.0f);
        }

        private float[] checkPointCollision() {
            float[] empty = {0f, 0f, -1f};
            foreach (var point in hitboxPoints)
            {
                float[] hit = MapBuilder.CheckCollision(GameLoop._trackName, point.X, point.Y);
                if (hit[2] != -1) {
                    if (hit[2] == 0) {
                        this._speed = (this._speed*(-1));
                        _collisionAnimActive = true;
                        _collisionAnimStartTime = GameLoop.CurrentTime;
                        _collisionAnimPosition = point;
                    }
                    if(hit[2] == 102) {
                        GameLoop.HandleLapping(102);
                                            }
                    if(hit[2] == 127) {
                        GameLoop.HandleLapping(127);                  
                    }
                }
            }
            return empty;
        }

        public float getSpeed()
        {
            return _speed * 100000f; //Arbitrary scaling to make speed more legible
        }

        public GameObject GetGameObject() => _car;

        ~Car()
        {

        }
    }
}
