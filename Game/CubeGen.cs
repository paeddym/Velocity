using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Velocity {
    public class CubeGen {
        
        private Shader _shader;
        private float _posX, _posY, _posZ;
        private int _VAO;

        public CubeGen(float posX, float posY, float posZ, int VAO,Shader shader) {
           _posX = posX;
           _posY = posY;
           _posZ = posZ;

           _shader = shader;

           _VAO = VAO;
       }
        public Vector3 GetPosition()
        {
            return new Vector3(_posX, _posY, _posZ);
        }

        public Vector2 GetSize()
        {
            return new Vector2(1.0f, 1.0f); // Assume unit size unless you scale
        }


        public void Draw() {
            int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            GL.BindVertexArray(_VAO);

            Matrix4 model = Matrix4.CreateTranslation(_posX, _posY, _posZ);
            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            //GL.BindVertexArray(0);
            
            // This would be the draw call for rotating, but i do not use it at the moment
            //model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            //model = model * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time));
            //model = model * Matrix4.CreateTranslation(1.0f, 0.0f, -5.0f);
            //GL.UniformMatrix4(modelLocation, true, ref model);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
       }
    }
}

