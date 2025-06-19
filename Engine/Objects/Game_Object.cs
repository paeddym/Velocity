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
        private Vector2 front = new Vector2(.0f, .0f);

        private int VertexBufferObject = -1;
        private int VertexArrayObject = -1;
        private int ElementBufferObject = -1;

        public GameObject(string objectName, string shader, string texture) 
            : this(objectName, 

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
            
            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            Matrix4 model = Matrix4.CreateRotationZ(objectPos.Z);

            model = model * Matrix4.CreateTranslation(objectPos.X, objectPos.Y, 0.0f);
            GL.UniformMatrix4(modelLocation, true, ref model);

            if(VertexBufferObject == -1) {
                Shapes.BindQuad();
            } else {
                // ToDo: Implement the things that need to happen, when custom Shapes are used
            }

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

