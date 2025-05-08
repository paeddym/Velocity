using OpenTK.Graphics.OpenGL4;

// ToDo: Need to figure out on how to init this once an have it reusable in the whole Project!!!

// The idee is that the ObjectManger will use the information stored inside of an GameObject
// To set everything up in the OpenGL context so the GameObject can be drawn.
// There is list with all GameObjects, as all objects have a name you can reverenze them with a
// name probaply something like this ObjectManager objects;
// Game object = objects.GetGameObject("<ObjectName>");
// object.UpdatePosition(x, y, z, r);

namespace Engine {
    public class ObjectManager {
        // These are the default VBOs, VAOs, EBOs as we only
        // use Quads so we do not need to create to much
        // maby this is a good idee maby not.
        private float[] vertices = {
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };
        private uint[] _indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 3, 2    // second triangle
        };
        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;

        public ObjectManager() {
           VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length *  sizeof(float),
                    vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint),
                    _indices, BufferUsageHint.StaticDraw);
        }

        ~ObjectManager(){
        }
    }
}

