using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine {
    // The GameObject class contains all the information about a object this is:
    // Texture, Shader, 2D Position, rotation;
    // What maby needs to be added in the future is the Scaling if needed
    public class UIObject {
        public Vector4 objectPos = new Vector4(.0f, .0f, .0f, .0f);
        public Vector2 front = new Vector2(.0f, .0f);

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;

        private float[] vertices = {
            0.5f,  0.5f, 0.0f, .115f, 1.0f, // top right
            0.5f, -0.5f, 0.0f, .115f, .5f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.075f, .5f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.075f, 1.0f  // top left
        };

        private uint[] _indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 3, 2    // second triangle
        };

        private Shader _shader;
        private Texture _texture; 

        public UIObject() {

            _shader = new Shader("shaders/textUI.vert", "shaders/textUI.frag");
            _texture = new Texture("recources/Fonts/default_font.png");
            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();

            _shader.Use();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length *  sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Position (location = 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // TexCoord (location = 1)
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
 
        }

        public void Draw() {
            _shader.Use();
            _texture.Use();
            GL.BindVertexArray(VertexArrayObject);

            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(
                    0.0f, 800.0f,
                    600.0f, 0.0f,
                    -1.0f, 1.0f
                    );

            Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180.0f)) * 
                Matrix4.CreateScale(250.0f) * 
                Matrix4.CreateTranslation(400.0f, 300.0f, 0.0f);

            // Set uniforms
            int projLoc = GL.GetUniformLocation(_shader.Handle, "projection");
            GL.UniformMatrix4(projLoc, false, ref projection);

            int modelLoc = GL.GetUniformLocation(_shader.Handle, "model");
            GL.UniformMatrix4(modelLoc, false, ref model); 
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}


