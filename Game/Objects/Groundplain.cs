using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Engine;

namespace GameApp{
    public class Groundplain {
        private float[] vertices = {
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };

        private uint[] _indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 3, 2    // second triangle
        };
        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;

        private float _posX, _posY, _posZ;

        private Shader _shader;

        public Groundplain(float posX, float posY, float posZ, Shader shader) {
            _shader = shader;

            _posX = posX;
            _posY = posY;
            _posZ = posZ;

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length *  sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.EnableVertexAttribArray(0); 

            //GL.BindVertexArray(0);
        }

        public void Draw(){
            GL.BindVertexArray(VertexArrayObject);

            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            Matrix4 model = Matrix4.CreateTranslation(_posX, _posY, _posZ);
            GL.UniformMatrix4(modelLocation, true, ref model);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.BindVertexArray(0);
        }

        ~Groundplain()
        {

        }
    }
}
