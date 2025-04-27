using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Velocity {
    public class Car {
        private float[] vertices = {
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private uint[] _indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 3, 2    // second triangle
        };
        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;

        private float _posX = 0f;
        private float _posY = 0f; 
        private float _posZ = 0f;

        private float _rotZ = 0f;

        private Vector2 front = new Vector2(0.0f, 0.0f);

        private Shader _shader;
        private Camera _camera;

        private float _speed = 0f;

        public Car(Shader shader, Camera camera) {
            _shader = shader;
            _camera = camera;

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length *  sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            var vertexLocation = GL.GetAttribLocation(_shader.Handle,"aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = GL.GetAttribLocation(_shader.Handle,"aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));


            GL.EnableVertexAttribArray(0); 

            //GL.BindVertexArray(0);
        }

        public void Draw(){
            GL.BindVertexArray(VertexArrayObject);

            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            Matrix4 model = Matrix4.CreateRotationZ(_rotZ);
            front.X = (float)Math.Sin(-1 * _rotZ);
            front.Y = (float)Math.Cos(_rotZ);

            //front = Vector2.Normalize(front);

            //Console.WriteLine("posX: " + _posX);
            //Console.WriteLine("posY: " + _posY);


            _posX = _posX + front.X * _speed;
            _posY = _posY + front.Y * _speed;

            model = model * Matrix4.CreateTranslation(_posX, _posY, _posZ);
            //Console.WriteLine("rotX: " + front.X);
            //Console.WriteLine("rotY: " + front.Y);
            GL.UniformMatrix4(modelLocation, true, ref model);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.BindVertexArray(0);
        }
        public void Drive(KeyboardState input, FrameEventArgs even, bool isFree) {
            float _deltaTime = (float)even.Time;
            float maxSpeed = 5f * _deltaTime;
            float revMaxSpeed = -2.5f * _deltaTime;

            // Constants
            float acceleration = 0.001f;     // Acceleration rate
            float deceleration = 0.00005f;     // Reverse acceleration rate

            // Apply input
            if (input.IsKeyDown(Keys.W))
            {
                _speed += acceleration * _deltaTime;
                if (_speed > maxSpeed) _speed = maxSpeed;
            }
            else if (input.IsKeyDown(Keys.S))
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
            if (input.IsKeyDown(Keys.A) && _speed != 0)
            {
                //_posX -= 1f * _delatTime;
                _rotZ += 1.5f * _deltaTime;
            }

            if (input.IsKeyDown(Keys.D) && _speed != 0)
            {
                //_posX += 1f * _delatTime;
                _rotZ -= 1.5f * _deltaTime;
            }

            if (!isFree) {
                _camera.UseLockCam(_posX, _posY);
            }

            Console.WriteLine(_speed/_deltaTime);
        }
        
        public void BounceBack()
        {
            _speed = -_speed; 
        }

        public Vector3 GetPosition()
        {
            return new Vector3(_posX, _posY, _posZ);
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
