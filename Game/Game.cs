using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Engine;

namespace GameApp{
    public class Game : GameWindow {

        Camera _camera; 
        Car _car;

        string[] maps = {"map3"};

        public Game(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {}
        protected override void OnLoad(){

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            // Maby add back later
            //CursorState = CursorState.Grabbed;
            // Initialize the EngineCore aka default shader and texture
            // All other game initalisations need to be done after the Engine Init
            EngineCore.Initialize("shaders/default.vert", "shaders/default.frag", 
                    "recources/textures/container.jpg", true);

            ResourceManager.LoadShader("text", "shaders/textUI.vert", "shaders/textUI.frag");
            TextRenderer.Initialize();
            TextRenderer.GenerateFont("default", "recources/fonts/04B_30__.TTF");
            Shapes.Initialize();

            ResourceManager.LoadTexture("car", "recources/cars/Car_01.png");
            ResourceManager.LoadTexture("map3", "recources/tracks/Track_03.png");

            MapBuilder.Initialize(maps);

            //GameObject test1 = new GameObject("test");
            //ObjectManager.AddGameObject(test1);
            GameObject Map = new GameObject("map3", "map3");
            Map.scale.X = 40f;
            Map.scale.Y = 40f;
            ObjectManager.AddGameObject(Map);

            GameObject test = new GameObject("car", "car");
            ObjectManager.AddGameObject(test);

            
            
            _camera = new Camera();
            _car = new Car("car", _camera);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            ObjectManager.DrawAll();
            //RenderText(string shader, string text, float x, float y, float scale, Vector3 color)
            Vector3 color = new Vector3(0.5f, 0.8f, 0.2f);
            TextRenderer.RenderText("text", "This is a test Text xD", 25f, 25f, 1f, color);
            TextRenderer.RenderText("text", "This is a test Text xD", 25f, 500f, 1f, color);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);

            InputProvider.UpdateInputStates(KeyboardState, e, MouseState, IsFocused);
            _car.Drive();

            if (KeyboardState.IsKeyDown(Keys.Escape)){
                Close();
            }
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
        }

    }
}

