using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine {
    public class Camera {

        // Camera
        Vector3 position;
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

        public Camera(Shader shader) {
            // Initial camera Setup 
            _shader = shader;
            position = new Vector3(0.0f, 0.0f, 3.0f);
            front = new Vector3(0.0f, 0.0f, -1.0f);
            up = new Vector3(0.0f, 1.0f, 0.0f);
            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, true, ref view);
        }

        public void UseFreeCam(KeyboardState input, FrameEventArgs even, MouseState mouse, bool windoFocus){
            float speed = 1.5f;
            float _delatTime = (float)even.Time; 

            if (needReset) {
                lastPos = new Vector2(mouse.X, mouse.Y);
                resetCam();
                needReset = false;
            }

            if (!windoFocus || input.IsKeyDown(Keys.RightShift)) {// check to see if the window is focused and right shift is pressed
                return;
            }

            if (input.IsKeyDown(Keys.W)){
                position += front * speed * (float)_delatTime; //Forward 
            }

            if (input.IsKeyDown(Keys.S)){
                position -= front * speed * (float)_delatTime; //Backwards
            }

            if (input.IsKeyDown(Keys.A)){
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)_delatTime; //Left
            }

            if (input.IsKeyDown(Keys.D)){
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)_delatTime; //Right
            }

            if (input.IsKeyDown(Keys.Space)){
                position += up * speed * (float)_delatTime; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift)){
                position -= up * speed * (float)_delatTime; //Down
            }

            // Mouse input
            float deltaX = mouse.X - lastPos.X;
            float deltaY = mouse.Y - lastPos.Y;
            lastPos = new Vector2(mouse.X, mouse.Y);

            yaw += deltaX * 0.05f;
            pitch -= deltaY * 0.05f;

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);

            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, true, ref view);
        }

        public void UseLockCam(float x, float y) {
            needReset = true;

            position = new Vector3(x, y, 6.0f);
            front = new Vector3(0.0f, 0.0f, -1.0f);
            up = new Vector3(0.0f, 1.0f, 0.0f);
            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, true, ref view);
        }

        private void resetCam() {
            yaw = -90;
            pitch = 0;
            front = new Vector3(0.0f, 0.0f, -1.0f);
            up = new Vector3(0.0f, 1.0f, 0.0f);
            Matrix4 view = Matrix4.LookAt(position, position + front, up);
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, true, ref view);
        }

        ~Camera()
        {

        }
    }
}
