using System.Runtime.InteropServices;
using FreeTypeSharp;
using static FreeTypeSharp.FT;
using static FreeTypeSharp.FT_LOAD;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Engine {

    public static unsafe class TextRenderer{

        private static int VAO, VBO;

        private struct Character
        {
            public uint TextureID;
            public Vector2 Size;
            public Vector2 Bearing;
            public uint Advance;
        }

        private static Dictionary<char, Character> Characters = new();

        public static void Initialize()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            // 6 vertices × 4 floats each = 24 floats
            GL.BufferData(BufferTarget.ArrayBuffer, 6 * 4 * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public static int GenerateFont(string fontName, string ttfPath) {
            FT_LibraryRec_* lib;
            FT_FaceRec_* face;

            if (FT_Init_FreeType(&lib) != 0)
            {
                Console.WriteLine("Error: FreeType: Could not init FreeType Library");
                return -1;
            }

            IntPtr fontPathPtr = Marshal.StringToHGlobalAnsi(ttfPath);
            if (FT_New_Face(lib, (byte*)fontPathPtr, 0, &face) != 0)
            {
                Console.WriteLine("Error: FreeType: Failed to load font");
                return -1;
            }

            FT_Set_Pixel_Sizes(face, 0, 48);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            for (byte c = 0; c < 128; c++)
            {
                Console.WriteLine("Loading Char: " + (char)c);
                if (FT_Load_Char(face, (char)c, FT_LOAD_RENDER) != 0)
                {
                    Console.WriteLine($"Error: FreeType: Failed to load Glyph '{(char)c}'");
                    continue;
                }

                uint texture;
                GL.GenTextures(1, out texture);
                GL.BindTexture(TextureTarget.Texture2D, texture);

                var bitmap = face->glyph->bitmap;

                GL.TexImage2D(TextureTarget.Texture2D,
                        0,
                        PixelInternalFormat.R8,
                        (int)bitmap.width,
                        (int)bitmap.rows,
                        0,
                        PixelFormat.Red,
                        PixelType.UnsignedByte,
                        (IntPtr)bitmap.buffer);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                Character character = new Character
                {
                    TextureID = texture,
                    Size = new Vector2(bitmap.width, bitmap.rows),
                    Bearing = new Vector2(face->glyph->bitmap_left, face->glyph->bitmap_top),
                    Advance = (uint)face->glyph->advance.x
                };

                Characters.Add((char)c, character);
            }

            FT_Done_Face(face);
            FT_Done_FreeType(lib);
            Marshal.FreeHGlobal(fontPathPtr);

            return 0;
        }

        public static void RenderText(string shader, string text, float x, float y, float scale, Vector3 color) {
            Shader _shader = ResourceManager.GetShader(shader);
            _shader.Use();

            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0.0f, 800.0f, 0.0f, 600.0f, -1.0f, 1.0f);
            int projectionLocation =  GL.GetUniformLocation(_shader.Handle, "projection");
            GL.UniformMatrix4(projectionLocation, false, ref projection);

            GL.Uniform3(GL.GetUniformLocation(_shader.Handle, "textColor"), 
                    color.X, color.Y, color.Z);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(VAO);

            foreach(char c in text) {
                if (!Characters.ContainsKey(c)) continue;
                Character ch = Characters[c];
                float xpos = x + ch.Bearing.X * scale;
                float ypos = y - (ch.Size.Y - ch.Bearing.Y) * scale;

                float w = ch.Size.X * scale;
                float h = ch.Size.Y * scale;

                float[] vertices = {
                    xpos,     ypos + h,   0.0f, 0.0f,            
                    xpos,     ypos,       0.0f, 1.0f,
                    xpos + w, ypos,       1.0f, 1.0f,

                    xpos,     ypos + h,   0.0f, 0.0f,
                    xpos + w, ypos,       1.0f, 1.0f,
                    xpos + w, ypos + h,   1.0f, 0.0f           
                };

                GL.BindTexture(TextureTarget.Texture2D, ch.TextureID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertices.Length * sizeof(float), vertices);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

                x += (ch.Advance >> 6) * scale;
            }
            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}

