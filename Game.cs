using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


namespace Velocity{
    public class Game : GameWindow {
        //private readonly float[] _vertices =
        //{
        //    -0.5f, -0.5f, 0.0f, // Bottom-left vertex
        //    0.5f, -0.5f, 0.0f, // Bottom-right vertex
        //    0.0f,  0.5f, 0.0f  // Top vertex
        //};
        float[] _vertices = {
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
            0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
            0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
            0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };
        uint[] _indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        private int _vertexBufferObject;
        private int _vertexArrayObject;

        private Shader _shader;
        private Texture _texture;
        private Texture _texture2;
        private double _time;
        // Cube that can be moved
        private double _xPos;
        private double _yPos;
        private double _zPos;
        private double _xRot;
        private double _yRot;
        Matrix4 projection;
        Matrix4 model;

        public Game(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {}
        protected override void OnLoad(){
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _shader = new Shader("shader.vert", "shader.frag");
            _shader.Use();

            _texture = new Texture("container.jpg");
            _texture2 = new Texture("awesomeface.png");
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);


            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = GL.GetAttribLocation(_shader.Handle,"aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = GL.GetAttribLocation(_shader.Handle,"aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));



            _shader.SetInt("texture1", 0);
            _shader.SetInt("texture2", 1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800.0f / 600.0f, 0.1f, 100.0f);

            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            GL.UniformMatrix4(modelLocation, true, ref model);
            
            int viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, true, ref view);

            int projectionLocation =  GL.GetUniformLocation(_shader.Handle, "projection");
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            _time += 32.0 * e.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);

            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            GL.BindVertexArray(_vertexArrayObject);
            
            model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            model = model * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time));
            model = model * Matrix4.CreateTranslation(1.0f, 0.0f, -5.0f);
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(-_time));
            model = model * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(-_time));
            model = model * Matrix4.CreateTranslation(-1.0f, 0.0f, -2.0f);
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time*10));
            model = model * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time*40));
            model = model * Matrix4.CreateTranslation(0.0f, 1.0f, 0.0f);
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            moveCube();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape)){
                Close();
            }
        }

        void moveCube() {
            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");

            if (KeyboardState.IsKeyDown(Keys.W)){
                _yPos += 0.001f;
                if (_yPos >= 1) {
                    _yPos = 1;
                }
            }
            if (KeyboardState.IsKeyDown(Keys.S)){
                _yPos -= 0.001f;
                if (_yPos <= -1) {
                    _yPos = -1;
                }
            }
            if (KeyboardState.IsKeyDown(Keys.A)){
                _xPos -= 0.001f;
                if (_xPos <= -1) {
                    _xPos = -1;
                }
            }
            if (KeyboardState.IsKeyDown(Keys.D)){
                _xPos += 0.001f;
                if (_xPos >= 1) {
                    _xPos = 1;
                }
            }
            if (KeyboardState.IsKeyDown(Keys.R)){
                _yRot += 0.01f;
            }
            if (KeyboardState.IsKeyDown(Keys.F)){
                _yRot -= 0.01f;
            }
            if (KeyboardState.IsKeyDown(Keys.G)){
                _xRot += 0.01f;
            }
            if (KeyboardState.IsKeyDown(Keys.T)){
                _xRot -= 0.01f;
            }
            if (KeyboardState.IsKeyDown(Keys.Y)){ // OpenTK uses Amerikan english keyboard layout, so Z and Y are swapped
                _zPos -= 0.01f;
                if (_zPos <= -10f) {
                    _zPos = -10f;
                }
            }
            if (KeyboardState.IsKeyDown(Keys.H)){
                _zPos += 0.01f;
                if (_zPos >= 3f) {
                    _zPos = 3f;
                }
            }


            model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_xRot));
            model = model * Matrix4.CreateRotationY((float) MathHelper.DegreesToRadians(_yRot));
            model = model * Matrix4.CreateTranslation((float)_xPos, (float)_yPos, (float)_zPos);
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);           
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e){
            GL.Viewport(0, 0, e.Width, e.Height);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), e.Width / e.Height, 0.1f, 100.0f);
            base.OnFramebufferResize(e);
        }

        protected override void OnUnload(){
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);

            GL.DeleteProgram(_shader.Handle);

        }

    }
}
