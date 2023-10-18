using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default) ;                                                 //Window Settings

window.Size = new OpenTK.Mathematics.Vector2i(800, 800);

window.RenderFrame += Window_RenderFrame;
window.KeyDown += args =>
{
    switch (args.Key)
    {
        case Keys.Escape: window.Close(); break;
    }
};

void Window_RenderFrame(OpenTK.Windowing.Common.FrameEventArgs obj)
{
    window.SwapBuffers();                           
}

window.Run();                                    
