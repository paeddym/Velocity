using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Engine
{
    public static class AnimationRenderer
    {
        private static int VAO, VBO;

        public static void Initialize()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, 6 * 5 * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            // layout(location = 0) = vec3 aPosition
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // layout(location = 1) = vec2 aTexCoord
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public static void Draw(Texture texture, string shaderName, double currentTime, double duration, Box2 bounds, uint columns, uint rows)
        {
            if (currentTime >= duration)
            {
                return;
            }

            uint totalFrames = columns * rows;
            double progress = currentTime / duration;
            uint spriteId = (uint)Math.Min(progress * totalFrames, totalFrames - 1);

            Box2 texCoords = CalcTexCoords(spriteId, columns, rows);

            Shader shader = ResourceManager.GetShader(shaderName);
            shader.Use();

            // Uniform setup
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, 800, 0, 600, -1, 1);
            Matrix4 view = Matrix4.Identity;
            Vector3 scale = new(bounds.Size.X, bounds.Size.Y, 1f);
            Vector3 translation = new(bounds.Min.X + bounds.Size.X / 2f, bounds.Min.Y + bounds.Size.Y / 2f, 0f);
            Matrix4 model = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(translation);

            GL.UniformMatrix4(GL.GetUniformLocation(shader.Handle, "model"), false, ref model);
            GL.UniformMatrix4(GL.GetUniformLocation(shader.Handle, "view"), false, ref view);
            GL.UniformMatrix4(GL.GetUniformLocation(shader.Handle, "projection"), false, ref projection);

            float[] vertices = {
                // pos (x,y,z)         // tex (u,v)
                -0.5f,  0.5f, 0f,     texCoords.Min.X, texCoords.Max.Y,
                -0.5f, -0.5f, 0f,     texCoords.Min.X, texCoords.Min.Y,
                 0.5f, -0.5f, 0f,     texCoords.Max.X, texCoords.Min.Y,

                -0.5f,  0.5f, 0f,     texCoords.Min.X, texCoords.Max.Y,
                 0.5f, -0.5f, 0f,     texCoords.Max.X, texCoords.Min.Y,
                 0.5f,  0.5f, 0f,     texCoords.Max.X, texCoords.Max.Y,
            };

            GL.ActiveTexture(TextureUnit.Texture0); // default sampler = texture1
            texture.Use();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertices.Length * sizeof(float), vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private static Box2 CalcTexCoords(uint spriteId, uint columns, uint rows)
        {
            uint row = spriteId / columns;
            uint col = spriteId % columns;
            float x = col / (float)columns;
            float y = 1f - ((row + 1f) / (float)rows);
            float width = 1f / columns;
            float height = 1f / rows;
            return new Box2(x, y, x + width, y + height);
        }
    }
}
