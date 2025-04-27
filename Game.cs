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
        private Shader _carShader;
        private Texture _texture;
        private Texture _texture2;
        private Texture _texture3;
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
        CubeGen cube4;

        Camera camera;

        CollisionManager collisionManager;

        Car car;

        private bool cameraFree = true;

        public Game(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {}
        protected override void OnLoad(){

            ErrorChecker.InitializeGLDebugCallback();
            // Clear buffer color
            // Enable depth test so objects in the backround don't shine trhough objects in fornt
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
            // Create, compile and use the vertex and Fragmet shader
            
            _carShader = new Shader("shaders/car_shader.vert", "shaders/car_shader.frag");
            _carShader.Use();
            _texture3 = new Texture("recources/Car_01.png");
            _carShader.SetInt("texture0", 0);

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
            cube1 = new CubeGen(3.0f, 4.0f, 0.0f, _vertexArrayObject, _shader);
            cube2 = new CubeGen(-3.0f, -2.0f, 0.0f, _vertexArrayObject, _shader);
            cube3 = new CubeGen(-5.0f, 0.0f, 0.0f, _vertexArrayObject, _shader);
            cube4 = new CubeGen(2.0f, -2.0f, 0.0f, _vertexArrayObject, _shader);
            List<CubeGen> cubes = new List<CubeGen>
            {
                cube1,
                cube2,
                cube3,
                cube4
            };

            camera = new Camera(_shader);

            car = new Car(_shader, camera);

            collisionManager = new CollisionManager(car, cubes);

            CursorState = CursorState.Grabbed;
            
            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            _time += 10.0f *  e.Time;
            _delatTime = e.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            _shader.SetInt("useSecTex", 1);
            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);

            cube1.Draw();
            cube2.Draw();
            cube3.Draw();
            cube4.Draw();
            testGround.Draw();

            //_carShader.Use();
            _shader.SetInt("useSecTex", 0);
            _texture3.Use();
            car.Draw();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);
            //UseFreeCam(KeyboardState input, FrameEventArgs even, MouseState mouse, bool windoFocus)
            if (cameraFree) {
                camera.UseFreeCam(KeyboardState, e, MouseState, IsFocused);
            }
            if (!cameraFree) {
                car.Drive(KeyboardState, e, cameraFree);
            }

            if (KeyboardState.IsKeyPressed(Keys.B)) {
                cameraFree = !cameraFree;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape)){
                Close();
            }

            collisionManager.CheckCollisions();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e){
            // Maintain a fixed aspect ratio (e.g., 800:600)
            float targetAspect = 800f / 600f;
            float windowAspect = (float)e.Width / e.Height;

            int vpWidth = e.Width;
            int vpHeight = e.Height;

            if (windowAspect > targetAspect)
            {
                // Window is too wide, add horizontal bars
                vpWidth = (int)(e.Height * targetAspect);
                GL.Viewport((e.Width - vpWidth) / 2, 0, vpWidth, e.Height);
            }
            else
            {
                // Window is too tall, add vertical bars
                vpHeight = (int)(e.Width / targetAspect);
                GL.Viewport(0, (e.Height - vpHeight) / 2, e.Width, vpHeight);
            }

    // Keep projection unchanged
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
