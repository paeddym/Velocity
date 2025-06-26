using System.Runtime.InteropServices;
using FreeTypeSharp;
using static FreeTypeSharp.FT;
using static FreeTypeSharp.FT_LOAD;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Engine {
    public static unsafe class TextRenderer
    {
        struct Character
        {
            public uint TextureID;
            public Vector2 Size;
            public Vector2 Bearing;
            public uint Advance;
        }

        static Dictionary<char, Character> Characters = new();

        public static int GenerateFont(string fontName, string ttfPath)
        {
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
                if (FT_Load_Char(face, c, FT_LOAD_RENDER) != 0)
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
    }
}

