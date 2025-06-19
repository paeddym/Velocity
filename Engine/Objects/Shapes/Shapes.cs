// TODO: Make this Class static an contain default shapes, like Quad, Rectangle, Circle 
using OpenTK.Graphics.OpenGL4;

namespace Engine {
    public static class Shapes{
        // These are the default VBOs, VAOs, EBOs as we only
        // use Quads so we do not need to create to much
        // maby this is a good idee maby not.
        private static float[] vertices = {
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private static uint[] _indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 3, 2    // second triangle
        };

        private static int VertexBufferObject = GL.GenBuffer();
        private static int VertexArrayObject = GL.GenVertexArray();
        private static int ElementBufferObject = GL.GenBuffer();

        public static void Initialize() {
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length *  sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        }

        public static void BindQuad() {
            GL.BindVertexArray(VertexArrayObject);
        }
        public static int GetQuadIndices() {
            return _indices.Length;
        }
    }
 }
