// TODO: Make this Class static an contain default shapes, like Quad, Rectangle, Circle 

namespace Engine {
    public class Shapes{
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
    }
 }
