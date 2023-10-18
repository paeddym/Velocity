using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

GameWindow window = new(GameWindowSettings.Default, NativeWindowSettings.Default) ;                                                 //Window Settings

window.Size = new OpenTK.Mathematics.Vector2i(800, 800);
//window.Resize += args => View.Resize(args.Width, args.Height); -> Resize Implementieren
//window.RenderFrame += _ => draw...    -> Draw implementieren
window.RenderFrame += _ => window.SwapBuffers();
window.Run();                                    
