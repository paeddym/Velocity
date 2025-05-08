using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

// The idee is that an object contains all the information needed to draw an object.
// The ObjectManger then will use this information to set everything up so that the object can
// be drawn.

namespace Engine {
    public class GameObject {
        String _objectName;
        Shader _shader;
        Texture _texture;

        private float _posX = 0f;
        private float _posY = 0f; 
        private float _posZ = 0f;

        private float _rotZ = 0f;

        private Vector2 front = new Vector2(0.0f, 0.0f);

        private int VertexBufferObject = 0;
        private int VertexArrayObject = 0;
        private int ElementBufferObject = 0;

        public GameObject(String objectName, Shader shader, Texture texture) {
            this._objectName = objectName;
            this._shader = shader;
            this._texture = texture;
        }
        public GameObject(String objectName, Shader shader, Texture texture, int vbo,
                int vao, int ebo) {

            this._objectName = objectName;
            this._shader = shader;
            this._texture = texture;
            this.VertexArrayObject = vao;
            this.VertexBufferObject = vao;
            this.ElementBufferObject = ebo;
        }

        public void UpdatePosition(float posX, float posY, float posZ, float rotZ) {
            this._posX = posX;
            this._posY = posY;
            this._posZ = posZ;
            this._rotZ = rotZ;
        }

        ~GameObject(){
        }
    }
}

