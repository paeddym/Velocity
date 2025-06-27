using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Engine;

namespace GameApp{
    public class Car {

        private float _speed = 0f;
        private string _name;
        private Camera _camera;

        public Car(string name, Camera camera) {
            this._name = name;
            this._camera = camera;
        }

        public void Drive() {
            FrameEventArgs _event = InputProvider.GetFrameEvent();
            float _deltaTime = (float)_event.Time;
            KeyboardState _keyboardState = InputProvider.GetKeyboardState();
            float maxSpeed = 5f * _deltaTime;
            float revMaxSpeed = -2.5f * _deltaTime;

            GameObject car = ObjectManager.GetGameObject(this._name);
            car.objectPos.Z = .1f;

            // Constants
            float acceleration = 0.001f;     // Acceleration rate
            float deceleration = 0.00005f;     // Reverse acceleration rate

            // Apply input
            if (_keyboardState.IsKeyDown(Keys.W))
            {
                _speed += acceleration * _deltaTime;
                if (_speed > maxSpeed) _speed = maxSpeed;
            }
            else if (_keyboardState.IsKeyDown(Keys.S))
            {
                _speed -= acceleration * _deltaTime;
                if (_speed < revMaxSpeed) _speed = revMaxSpeed;
            }
            else
            {
                // Optional: natural friction to stop when no input
                if (_speed > 0)
                {
                    _speed -= acceleration * _deltaTime;
                    if (_speed < 0) _speed = 0;
                }
                else if (_speed < 0)
                {
                    _speed += acceleration * _deltaTime;
                    if (_speed > 0) _speed = 0;
                }
            }

            // Adjust steering based on whether the car is moving forward or backward
            if (_keyboardState.IsKeyDown(Keys.A) && _speed != 0)
            {
                if (_speed > 0)
                {
                    car.objectPos.W += 1.5f * _deltaTime;
                }
                else
                {
                    car.objectPos.W -= 1.5f * _deltaTime;
                }
            }

            if (_keyboardState.IsKeyDown(Keys.D) && _speed != 0)
            {
                if (_speed > 0)
                {
                    car.objectPos.W -= 1.5f * _deltaTime;
                }
                else
                {
                    car.objectPos.W += 1.5f * _deltaTime;
                }
            }

            car.front.X = (float)Math.Sin(-1 * car.objectPos.W);
            car.front.Y = (float)Math.Cos(car.objectPos.W);

            //_posX = _posX + front.X * _speed;
            //_posY = _posY + front.Y * _speed;
            car.objectPos.X = car.objectPos.X + car.front.X * _speed;
            car.objectPos.Y = car.objectPos.Y + car.front.Y * _speed;

            _camera.UseLockCam(car.objectPos.X, car.objectPos.Y, car.objectPos.W);

        }

        public void BounceBack()
        {
            Console.WriteLine("Car bounce back");
            _speed = -_speed; 
        }

        // Optional: size if needed
        public Vector2 GetSize()
        {
            return new Vector2(1.0f, 1.0f);
        }


        ~Car()
        {

        }
    }
}
