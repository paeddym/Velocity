using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Engine;

namespace GameApp{
    public class Game : GameWindow {

        Camera _camera; 
        Car _car;

        public Game(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {}
        protected override void OnLoad(){

            // Clear buffer color
            // Enable depth test so objects in the backround don't shine trhough objects in fornt
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //CursorState = CursorState.Grabbed;
            // Initialize the EngineCore aka default shader and texture
            // All other game initalisations need to be done after the Engine Init
            //EngineCore.Initialize("shaders/default.vert", "shaders/default.frag", 
            //        "recources/textures/container.jpg", true);
            Shapes.Initialize();

            //ResourceManager.LoadTexture("car", "recources/textures/Car_01.png");

            //GameObject test1 = new GameObject("test");
            //ObjectManager.AddGameObject(test1);

            //GameObject test = new GameObject("car", "car");
            //ObjectManager.AddGameObject(test);

            //_camera = new Camera();
            //_car = new Car("car", _camera);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //ObjectManager.DrawAll();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);

            //InputProvider.UpdateInputStates(KeyboardState, e, MouseState, IsFocused);
            //_car.Drive();

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

