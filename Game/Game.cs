using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Engine;

namespace GameApp{
    public class Game : GameWindow {

        private int _mapSelection = 1;

        string[] tracks = {"track1hitbox","track2hitbox","track3hitbox"};
        private string[] _cars = {"recources/cars/Car_01.png",
                                  "recources/cars/Car_02.png",
                                  "recources/cars/Car_03.png",};

        private string[] _tracks={"recources/tracks/Track_01.png",
                                  "recources/tracks/Track_02.png",
                                  "recources/tracks/Track_03.png",};

        private string[] _trackHitbox={"recources/tracks/Track_01_hitbox.png",
                                       "recources/tracks/Track_02_hitbox.png",
                                       "recources/tracks/Track_03_hitbox.png",};

        private string[] _animations = {"recources/spritesheets/SmokeEffect.png"};

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



            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector3 color = new Vector3(0.5f, 0.8f, 0.2f);
       
            if (GameStateManager.IsState(GameStateManager.GameState.MainMenu)){
                TextRenderer.RenderText("text", "Welcome to Velocity", 130f, 500f, 0.7f, color);
                TextRenderer.RenderText("text", "be a racer", 200f, 480f, 0.4f, color);
                TextRenderer.RenderText("text", "Press Enter to Start", 110f, 300f, 0.7f, color);
                TextRenderer.RenderText("text", "Press Esc to Quit", 20f, 20f, 0.5f, color);
            }
            if (GameStateManager.IsState(GameStateManager.GameState.MapSelection)){                
                TextRenderer.RenderText("text", "Select a map to race", 130f, 500f, 0.7f, color);
                TextRenderer.RenderText("text", "use the arrow keys", 200f, 480f, 0.4f, color);
                if(_mapSelection == 1){
                    TextRenderer.RenderText("text", "Track 01 (easy)<", 130f, 400f, 0.7f, color);
                    TextRenderer.RenderText("text", LapTimeStorage.LoadBestLapTime("track1") is double t ? $"Best Time: {FormatHelper.FormatTime(t)}" : "", 130f, 360f, 0.5f, color);
                    TextRenderer.RenderText("text", "Track 02 (medium)", 130f, 300f, 0.7f, color);
                    TextRenderer.RenderText("text", "Track 03 (hard)", 130f, 200f, 0.7f, color);
                }
                if(_mapSelection == 2){
                    TextRenderer.RenderText("text", "Track 01 (easy)", 130f, 400f, 0.7f, color);
                    TextRenderer.RenderText("text", "Track 02 (medium)<", 130f, 300f, 0.7f, color);
                    TextRenderer.RenderText("text", LapTimeStorage.LoadBestLapTime("track2") is double t ? $"Best Time: {FormatHelper.FormatTime(t)}" : "", 130f, 260f, 0.5f, color);
                    TextRenderer.RenderText("text", "Track 03 (hard)", 130f, 200f, 0.7f, color);
                }
                if(_mapSelection == 3){
                    TextRenderer.RenderText("text", "Track 01 (easy)", 130f, 400f, 0.7f, color);
                    TextRenderer.RenderText("text", "Track 02 (medium)", 130f, 300f, 0.7f, color);
                    TextRenderer.RenderText("text", "Track 03 (hard)<", 130f, 200f, 0.7f, color);
                    TextRenderer.RenderText("text", LapTimeStorage.LoadBestLapTime("track3") is double t ? $"Best Time: {FormatHelper.FormatTime(t)}" : "", 130f, 160f, 0.5f, color);
                }
            }
            if (GameStateManager.IsState(GameStateManager.GameState.Playing)){
                ObjectManager.DrawAll();
                ParticleManager.Draw();

                var car = GameLoop.CarInstance;
                if (car.CollisionAnimActive)
                {
                    float zoom = 20f; // Use your game's zoom factor
                    var camera = GameLoop.CameraInstance;
                    // Get camera rotation (rotZ) from car's GameObject
                    float rotZ = car.GetGameObject().objectPos.W;
                    // Animation world position relative to camera
                    var rel = car.CollisionAnimPosition - new Vector2(camera.position.X, camera.position.Y);
                    // Rotate by -rotZ
                    float cos = (float)Math.Cos(-rotZ);
                    float sin = (float)Math.Sin(-rotZ);
                    float rx = rel.X * cos - rel.Y * sin;
                    float ry = rel.X * sin + rel.Y * cos;
                    // Convert to screen coordinates
                    float screenX = rx * zoom + 400;
                    float screenY = ry * zoom + 300;
                    var animScreenPos = new Vector2(screenX, screenY);

                    AnimationRenderer.Draw(
                        ResourceManager.GetTexture("animation1"),
                        "animation",
                        GameLoop.CurrentTime - car.CollisionAnimStartTime,
                        car.CollisionAnimDuration,
                        new Box2(animScreenPos - new Vector2(32, 32), animScreenPos + new Vector2(32, 32)),
                        11, 1
                    );
                }
                TextRenderer.RenderText("text", $"Speed: {GameLoop.CarInstance.getSpeed():F0}", 30f, 30f, 0.4f, color);
                TextRenderer.RenderText("text", $"Lap: {GameLoop.LapCount}/{GameLoop.MaxLaps}", 30f, 60f, 0.4f, color);
                TextRenderer.RenderText("text", $"Time: {FormatHelper.FormatTime(GameLoop.CurrentTime)}", 550f, 570f, 0.4f, color);
                TextRenderer.RenderText("text", GameLoop.TrackRecord < double.MaxValue ? $"Record: {FormatHelper.FormatTime(GameLoop.TrackRecord)}" : "", 30f, 570f, 0.4f, color);                
                TextRenderer.RenderText("text", GameLoop.BestLapTime < double.MaxValue ? $"Best: {FormatHelper.FormatTime(GameLoop.BestLapTime)}" : "", 550f, 540f, 0.4f, color);
                if(GameLoop.ShowSplits && GameLoop.ShowSplitsTimer > 0){
                    TextRenderer.RenderText("text", $"{FormatHelper.FormatSplitTime(GameLoop.SplitDifference)}s", 330f, 540f, 0.4f, GameLoop.SplitDifference < 0 ? color : new Vector3(0.8f, 0.2f, 0.2f));
                    GameLoop.ShowSplitsTimer -= e.Time;
                }

                if (GameLoop.CurrentState == GameLoop.LoopState.CountDown)
                {
                    int countdownValue = (int)MathF.Ceiling(GameLoop.CountdownTime);
                    string display = countdownValue > 0 ? countdownValue.ToString() : "GO!";
                    TextRenderer.RenderText("text", display, 380f, 300f, 2.0f, color);
                }
            }

            if(GameStateManager.IsState(GameStateManager.GameState.Finished)){
                ObjectManager.DrawAll();
                TextRenderer.RenderText("text", "Finished!", 280f, 500f, 0.7f, color);
                TextRenderer.RenderText("text", GameLoop.CurrentTime == GameLoop.TrackRecord ? "New track record!" : "", 160f, 400f, 0.7f, color);
                TextRenderer.RenderText("text", $"Finished in: {FormatHelper.FormatTime(GameLoop.CurrentTime)}", 150f, 340f, 0.7f, color);
                TextRenderer.RenderText("text", $"Best Lap: {FormatHelper.FormatTime(GameLoop.BestLapTime)}", 170f, 300f, 0.7f, color);
                TextRenderer.RenderText("text", "Press Esc to Quit Game", 20f, 20f, 0.3f, color);
                TextRenderer.RenderText("text", "Press Enter for Main Menu", 490f, 20f, 0.3f, color);
            }

            if (GameStateManager.IsState(GameStateManager.GameState.Paused)){
                ObjectManager.DrawAll();
                TextRenderer.RenderText("text", "Game Paused", 250f, 500f, 0.7f, color);
                TextRenderer.RenderText("text", "Press Esc to Resume", 130f, 300f, 0.7f, color);
                TextRenderer.RenderText("text", "Press Enter for Main Menu", 20f, 20f, 0.5f, color);
            }
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);

            InputProvider.UpdateInputStates(KeyboardState, e, MouseState, IsFocused);

            if (GameStateManager.IsState(GameStateManager.GameState.MainMenu)){
                if (KeyboardState.IsKeyPressed(Keys.Enter)){
                    GameStateManager.ChangeState(GameStateManager.GameState.MapSelection);
                }
                if (KeyboardState.IsKeyPressed(Keys.Escape)){
                   Close(); 
                }
            }
            else if (GameStateManager.IsState(GameStateManager.GameState.MapSelection)){
                if(KeyboardState.IsKeyPressed(Keys.Down)){
                    _mapSelection++;
                    if(_mapSelection > 3){
                        _mapSelection = 1;
                    }
                }
                if(KeyboardState.IsKeyPressed(Keys.Up)){
                    _mapSelection--;
                    if(_mapSelection < 1){
                        _mapSelection = 3;
                    }
                }
                if(KeyboardState.IsKeyPressed(Keys.Escape)){
                    _mapSelection = 1;
                    GameStateManager.ChangeState(GameStateManager.GameState.MainMenu);
                }
                if(KeyboardState.IsKeyPressed(Keys.Enter)){
                    GameLoop.InitGameLoop($"car{_mapSelection}", $"track{_mapSelection}");
                    GameStateManager.ChangeState(GameStateManager.GameState.Playing);
                }
            }
            else if (GameStateManager.IsState(GameStateManager.GameState.Playing)){
                GameLoop.UpdateGame();
                if (KeyboardState.IsKeyPressed(Keys.Escape))
                    GameStateManager.ChangeState(GameStateManager.GameState.Paused);
            }
            else if (GameStateManager.IsState(GameStateManager.GameState.Finished)){
                if (KeyboardState.IsKeyPressed(Keys.Escape))
                    Close();
                if (KeyboardState.IsKeyPressed(Keys.Enter)){
                    ParticleManager.Reset();
                    GameStateManager.ChangeState(GameStateManager.GameState.MainMenu);
                }
            }

            else if (GameStateManager.IsState(GameStateManager.GameState.Paused)){
                if (KeyboardState.IsKeyPressed(Keys.Escape))
                    GameStateManager.ChangeState(GameStateManager.GameState.Playing);
                if (KeyboardState.IsKeyPressed(Keys.Enter)){
                    ParticleManager.Reset();
                    GameStateManager.ChangeState(GameStateManager.GameState.MainMenu);
                }
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
            i = 1;
            foreach (string texture in _trackHitbox) {
                ResourceManager.LoadTexture($"track{i}hitbox", texture);
                i = i + 1;
            }
            i = 1;
            //Load animation spritesheets
            foreach (string animation in _animations) {
                ResourceManager.LoadTexture($"animation{i}", animation);
                i = i + 1;
            }
            //Load all shaders
            ResourceManager.LoadShader("default", "shaders/default.vert", "shaders/default.frag");
            ResourceManager.LoadShader("text", "shaders/textUI.vert", "shaders/textUI.frag");
            ResourceManager.LoadShader("animation", "shaders/animation.vert", "shaders/animation.frag");
            ResourceManager.LoadShader("particle", "shaders/particle.vert", "shaders/particle.frag");

            ResourceManager.LoadTexture("particleTex", "recources/particles/Dustcloud_small.png");
            ParticleManager.Initialize();
            AnimationRenderer.Initialize();

            TextRenderer.Initialize();
            TextRenderer.GenerateFont("default", _fonts[0]);

            Shapes.Initialize();
            MapBuilder.Initialize(tracks);
        }
    }
}

