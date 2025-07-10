using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine {
    public class Camera {
// WARNING: The Camera currently works only with the default shader
        // Camera
        public Vector3 position;
        Vector3 cameraTarget;
        Vector3 cameraDirection;
        Vector3 up;
        Vector3 front;
        Vector3 cameraRight;
        Vector3 cameraUp;
        Vector2 lastPos;
        private float yaw = -90;
        private float pitch;
        private Shader _shader;

        private bool needReset = false;

        public Camera()
            : this(ResourceManager.GetShader("default")) {}

        public Camera(Shader shader) {
            // Initial camera Setup 
            this._shader = shader;
            _shader.Use();
            position = new Vector3(0.0f, 0.0f, 100.0f);
            front = new Vector3(0.0f, 0.0f, -1.0f);
            up = new Vector3(0.0f, 1.0f, 0.0f);
            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800.0f / 600.0f, 0.1f, 100.0f);

            int projectionLocation =  GL.GetUniformLocation(_shader.Handle, "projection");
            GL.UniformMatrix4(projectionLocation, false, ref projection);
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, false, ref view);
        }

        public void UseFreeCam(){
            float speed = 1.5f;
            FrameEventArgs _event = InputProvider.GetFrameEvent();
            float _delatTime = (float)_event.Time;
            MouseState _mouseState = InputProvider.GetMouseState();
            KeyboardState _keyboardState = InputProvider.GetKeyboardState();

            if (needReset) {
                lastPos = new Vector2(_mouseState.X, _mouseState.Y);
                resetCam();
                needReset = false;
            }

            if (!InputProvider.GetWindowFocus() || _keyboardState.IsKeyDown(Keys.RightShift)) {// check to see if the window is focused and right shift is pressed
                return;
            }

            if (_keyboardState.IsKeyDown(Keys.W)){
                position += front * speed * (float)_delatTime; //Forward 
            }

            if (_keyboardState.IsKeyDown(Keys.S)){
                position -= front * speed * (float)_delatTime; //Backwards
            }

            if (_keyboardState.IsKeyDown(Keys.A)){
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)_delatTime; //Left
            }

            if (_keyboardState.IsKeyDown(Keys.D)){
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)_delatTime; //Right
            }

            if (_keyboardState.IsKeyDown(Keys.Space)){
                position += up * speed * (float)_delatTime; //Up 
            }

            if (_keyboardState.IsKeyDown(Keys.LeftShift)){
                position -= up * speed * (float)_delatTime; //Down
            }

            // Mouse input
            float deltaX = _mouseState.X - lastPos.X;
            float deltaY = _mouseState.Y - lastPos.Y;
            lastPos = new Vector2(_mouseState.X, _mouseState.Y);

            yaw += deltaX * 0.05f;
            pitch -= deltaY * 0.05f;

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);

            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, false, ref view);
        }

        public void UseLockCam(float x, float y, float rotZ) {
            needReset = true;
            _shader.Use();

            position = new Vector3(x, y, 15.0f);

            Matrix4 rotationZ = Matrix4.CreateRotationZ(rotZ);

            front = Vector3.TransformVector(new Vector3(0.0f, 0.0f, -1.0f), rotationZ);
            up    = Vector3.TransformVector(new Vector3(0.0f, 1.0f,  0.0f), rotationZ);

            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, false, ref view);
        }


        private void resetCam() {
            yaw = -90;
            pitch = 0;
            front = new Vector3(0.0f, 0.0f, -1.0f);
            up = new Vector3(0.0f, 1.0f, 0.0f);
            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, false, ref view);
        }

        ~Camera()
        {

        }
    }
}
