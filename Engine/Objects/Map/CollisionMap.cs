using StbImageSharp;

namespace Engine {
    public class CollisionMap {
        public readonly bool[,] solidMap;
        public readonly int width;
        public readonly int height;

        public CollisionMap(string texName) {
            Texture texture = ResourceManager.GetTexture(texName);
            ImageResult image = texture.image;

            int width = image.Width;
            int height = image.Height;
            solidMap = new bool[width, height];

            byte[] data = image.Data;

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int index = (y * width + x) * 4;
                    byte alpha = data[index + 3];
                    solidMap[x, y] = alpha > 0;
                }
            }
        }

        public bool IsSolid(float worldX, float worldY, float mapPosX, float mapPosY, float mapWidth, float mapHeight) {
            // Convert world position to texture-space
            float relX = (worldX - mapPosX + mapWidth / 2) / mapWidth;
            float relY = (worldY - mapPosY + mapHeight / 2) / mapHeight;

            int texX = (int)(relX * width);
            int texY = (int)(relY * height);

            if (texX < 0 || texY < 0 || texX >= width || texY >= height)
                return false;

            return solidMap[texX, texY];
        }
    }
}
