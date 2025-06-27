using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine {
    // The GameObject class contains all the information about a object this is:
    // Texture, Shader, 2D Position, rotation;
    // What maby needs to be added in the future is the Scaling if needed
    public class GameObject {
        public String objectName;
        // Strucure posX, posY, posZ, rotZ
        public Vector4 objectPos = new Vector4(.0f, .0f, .0f, .0f);
        public Vector2 front = new Vector2(.0f, .0f);

        private Shader _shader;
        private Texture _texture;

        private int VertexBufferObject = -1;
        private int VertexArrayObject = -1;
        private int ElementBufferObject = -1;

        public GameObject(string objectName)
            : this(objectName, "default", "default") {}

        public GameObject(string objectName, string textureName) 
            : this(objectName, "default", textureName) {} 

        public GameObject(string objectName, string shaderName, string textureName) 
            : this(objectName, ResourceManager.GetShader(shaderName),
                    ResourceManager.GetTexture(textureName)) {}

        public GameObject(string objectName, Shader shader, Texture texture) {
            this.objectName = objectName;
            this._shader = shader;
            this._texture = texture;
        }

        public GameObject(string objectName, Shader shader, Texture texture, int vbo,
                int vao, int ebo) {

            this.objectName = objectName;
            this._shader = shader;
            this._texture = texture;
            this.VertexBufferObject = vbo;
            this.VertexArrayObject = vao;
            this.ElementBufferObject = ebo;
        }

        public void Draw() {
            this._shader.Use();
            this._texture.Use();

            if(VertexBufferObject == -1) {
                Shapes.BindQuad();
            } else {
                // ToDo: Implement the things that need to happen, when custom Shapes are used
            }

            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            Matrix4 model = Matrix4.CreateRotationZ(objectPos.W);

            model = model * Matrix4.CreateTranslation(objectPos.X, objectPos.Y, objectPos.Z);

            GL.UniformMatrix4(modelLocation, false, ref model);

            GL.DrawElements(PrimitiveType.Triangles, Shapes.GetQuadIndices(), DrawElementsType.UnsignedInt, 0);
        }

        ~GameObject(){
            if(this.VertexBufferObject != -1) {
                GL.DeleteVertexArray(this.VertexArrayObject);
            }

            if(this.VertexArrayObject != -1) {
                GL.DeleteBuffer(this.VertexBufferObject);
            }

            if(this.ElementBufferObject != -1) {
                GL.DeleteBuffer(this.ElementBufferObject);
            }
        }
    }
}

