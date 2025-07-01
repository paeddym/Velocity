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
        string[] tracks = {"track1","track2","track3"};
        private string[] _cars = {"recources/cars/Car_01.png",
                                  "recources/cars/Car_02.png",
                                  "recources/cars/Car_01.png",};

        private string[] _tracks={"recources/tracks/Track_01.png",
                                  "recources/tracks/Track_01.png",
                                  "recources/tracks/Track_01.png",};

        private string[] _fonts = {"recources/fonts/04B_30__.TTF"};

        public Game(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {}
        protected override void OnLoad(){

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            // Maby add back later
            //CursorState = CursorState.Grabbed;
            // Initialize the EngineCore aka default shader and texture
            // All other game initalisations need to be done after the Engine Init
            
            EngineCore.Initialize("recources/textures/container.jpg", true);
            InitializeGame();

            GameObject Map = new GameObject("track3", "track3");
            Map.scale = 40f;
            ObjectManager.AddGameObject(Map);

            GameObject test = new GameObject("car1", "car1");
            test.scale = 1f;
            ObjectManager.AddGameObject(test);

            
            
            _camera = new Camera();
            _car = new Car("car1", _camera);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 color = new Vector3(0.5f, 0.8f, 0.2f);

            if (GameStateManager.IsState(GameStateManager.GameState.MainMenu)){
                // Centered title
                TextRenderer.RenderText("text", "Press Enter to Start", 20f, 120f, 0.7f, color);
                // Centered instruction below
                TextRenderer.RenderText("text", "Press Esc to Quit", 20f, 20f, 0.7f, color);
            }
            else if (GameStateManager.IsState(GameStateManager.GameState.Playing)){
                ObjectManager.DrawAll();
                // Draw HUD, etc.
            }
            else if (GameStateManager.IsState(GameStateManager.GameState.Paused)){
                ObjectManager.DrawAll();
                // Centered "Paused"
                TextRenderer.RenderText("text", "Paused", 20f, 500f, 0.7f, color);
                // Instructions below
                TextRenderer.RenderText("text", "Press Esc to Resume", 20f, 20f, 0.7f, color);
                TextRenderer.RenderText("text", "Press Enter for Main Menu", 20f, 120f, 0.7f, color);
            }
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);

            InputProvider.UpdateInputStates(KeyboardState, e, MouseState, IsFocused);

            if (GameStateManager.IsState(GameStateManager.GameState.MainMenu))
            {
                if (KeyboardState.IsKeyPressed(Keys.Enter))
                    GameStateManager.ChangeState(GameStateManager.GameState.Playing);
                else if (KeyboardState.IsKeyPressed(Keys.Escape))
                    Close();
            }
            else if (GameStateManager.IsState(GameStateManager.GameState.Playing))
            {
                _car.Drive();
                if (KeyboardState.IsKeyPressed(Keys.Escape))
                    GameStateManager.ChangeState(GameStateManager.GameState.Paused);
            }
            else if (GameStateManager.IsState(GameStateManager.GameState.Paused))
            {
                if (KeyboardState.IsKeyPressed(Keys.Escape))
                    GameStateManager.ChangeState(GameStateManager.GameState.Playing);
                else if (KeyboardState.IsKeyPressed(Keys.Enter))
                    GameStateManager.ChangeState(GameStateManager.GameState.MainMenu);
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

        private void InitializeGame(){
            //Load all cars and tracks
            int i = 1;
            foreach (string texture in _cars) {
                ResourceManager.LoadTexture($"car{i}", texture);
                i = i + 1;
            }
            i = 1;
            foreach (string texture in _tracks) {
                ResourceManager.LoadTexture($"track{i}", texture);
                i = i + 1;
            }
            //Load all shaders
            ResourceManager.LoadShader("default", "shaders/default.vert", "shaders/default.frag");
            ResourceManager.LoadShader("text", "shaders/textUI.vert", "shaders/textUI.frag");

            TextRenderer.Initialize();
            TextRenderer.GenerateFont("default", _fonts[0]);

            Shapes.Initialize();
            MapBuilder.Initialize(tracks);
        }
    }
}

