using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

// The idee is that an object contains all the information needed to draw an object.
// The ObjectManger then will use this information to set everything up so that the object can
// be drawn.

namespace Engine {
    public class GameObject {
        public String objectName;
        // Strucure posX, posY, rotZ
        public Vector3 objectPos = new Vector3(.0f, .0f, .0f);

        private Shader _shader;
        private Texture _texture;
        private Vector2 front = new Vector2(0.0f, 0.0f);

        private int VertexBufferObject = 0;
        private int VertexArrayObject = 0;
        private int ElementBufferObject = 0;

        public GameObject(String objectName, Shader shader, Texture texture) {
            this.objectName = objectName;
            this._shader = shader;
            this._texture = texture;
        }
        public GameObject(String objectName, Shader shader, Texture texture, int vbo,
                int vao, int ebo) {

            this.objectName = objectName;
            this._shader = shader;
            this._texture = texture;
            this.VertexArrayObject = vao;
            this.VertexBufferObject = vao;
            this.ElementBufferObject = ebo;
        }

        public void Draw() {
            //Implement Draw() Function
        }

        ~GameObject(){
            //TODO: Implement cleanup logic for OpenGL resoruces
        }
    }
}

