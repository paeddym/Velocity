using System;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Engine {
    public class Texture {
        public readonly int Handle;

        public Texture(string path){
            Handle = GL.GenTexture();
            Use();

            StbImage.stbi_set_flip_vertically_on_load(1);

            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                    PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            Console.WriteLine(image);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);


            // Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
            // We set this to Repeat so that textures will repeat when wrapped. Not demonstrated here since the texture coordinates exactly match
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0){
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
