using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


namespace Velocity{
    public class Game : GameWindow {

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
        private double _delatTime;
        Matrix4 projection;
        Matrix4 model;

        Groundplain testGround;

        CubeGen cube1;
        CubeGen cube2;
        CubeGen cube3;

        Camera camera;

        public Game(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {}
        protected override void OnLoad(){

            ErrorChecker.InitializeGLDebugCallback();
            // Clear buffer color
            // Enable depth test so objects in the backround don't shine trhough objects in fornt
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            
            // Create, compile and use the vertex and Fragmet shader
            _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
            _shader.Use();
            // Load textures and use them
            _texture = new Texture("textures/container.jpg");
            _texture2 = new Texture("textures/awesomeface.png");
            _shader.SetInt("texture1", 0);
            _shader.SetInt("texture2", 1);

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

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800.0f / 600.0f, 0.1f, 100.0f);

            int projectionLocation =  GL.GetUniformLocation(_shader.Handle, "projection");
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            testGround = new Groundplain(1.5f, 0.0f, 0.0f, _shader);
            //CubeGen(float posX, float posY, float posZ, int VAO,Shader shader) 
            cube1 = new CubeGen(1.0f, 1.0f, 1.0f, _vertexArrayObject, _shader);
            cube2 = new CubeGen(-1.0f, -1.0f, -1.0f, _vertexArrayObject, _shader);
            cube3 = new CubeGen(-5.0f, 0.0f, 0.0f, _vertexArrayObject, _shader);
            
            camera = new Camera(_shader);

            CursorState = CursorState.Grabbed;
            
            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            _time += 10.0f *  e.Time;
            _delatTime = e.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);

            cube1.Draw();
            cube2.Draw();
            cube3.Draw();
            testGround.Draw();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);
            //Use(KeyboardState input, FrameEventArgs even, MouseState mouse, bool windoFocus)
            camera.Use(KeyboardState, e, MouseState, IsFocused);   

            if (KeyboardState.IsKeyDown(Keys.Escape)){
                Close();
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e){
            GL.Viewport(0, 0, e.Width, e.Height);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), e.Width / e.Height, 0.1f, 100.0f);
            //int projectionLocation =  GL.GetUniformLocation(_shader.Handle, "projection");
            //GL.UniformMatrix4(projectionLocation, true, ref projection);
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
