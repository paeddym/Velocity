using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using System.Diagnostics;

namespace Velocity{
    public class Game : GameWindow {
        //private readonly float[] _vertices =
        //{
        //    -0.5f, -0.5f, 0.0f, // Bottom-left vertex
        //    0.5f, -0.5f, 0.0f, // Bottom-right vertex
        //    0.0f,  0.5f, 0.0f  // Top vertex
        //};
        float[] _vertices = {
            -0.5f,  -0.5f, 0.0f,    1.0f, 0.0f, 0.0f, 
            0.5f, -0.5f, 0.0f,      0.0f, 1.0f, 0.0f, 
            0.0f, 0.5f, 0.0f,       0.0f, 0.0f, 1.0f, 
        };
        //uint[] _indices = {  // note that we start from 0!
        //    0, 1, 3,   // first triangle
        //    1, 2, 3    // second triangle
        //};

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        //private int _elementBufferObject;

        private Shader _shader;
        private Stopwatch _timer = new Stopwatch();

        public Game(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {}

        protected override void OnLoad(){
            _timer.Start();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            //_elementBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("shader.vert", "shader.frag");
            _shader.Use();

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
           // GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            //GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape)){
                Close();
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e){
            GL.Viewport(0, 0, e.Width, e.Height);
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
