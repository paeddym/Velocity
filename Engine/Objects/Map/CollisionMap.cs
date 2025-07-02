using StbImageSharp;

namespace Engine {
    public class CollisionMap {
        public readonly float[,] solidMap;
        public readonly int width;
        public readonly int height;

        public CollisionMap(string texName) {
            Texture texture = ResourceManager.GetTexture(texName);
            ImageResult image = texture.image;

            this.width = image.Width;
            this.height = image.Height;
            solidMap = new float[width, height];

            byte[] data = image.Data;

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int index = (y * width + x) * 4;
                    byte alpha = data[index + 3];
                    solidMap[x, y] = alpha; // Normalize to 0.0 - 1.0
                }
            }
        }

        /// Returns a float[3] where:
        /// [0] = world X
        /// [1] = world Y
        /// [2] = alpha at that point (0 = transparent, 1 = opaque, -1 = out of bounds)
        ///     = alpha at that point (0 = no track,    1 = track,  -1 = car out of map, 0.4 = start/finish line, 0.5 = chekcpoint line)
        public float[] IsSolid(float worldX, float worldY, float mapPosX, float mapPosY, float mapWidth, float mapHeight) {
            float[] collisionPos = { 0f, 0f, -1f };

            float relX = (worldX - mapPosX + mapWidth / 2f) / mapWidth;
            float relY = (worldY - mapPosY + mapHeight / 2f) / mapHeight;

            int texX = (int)(relX * width);
            int texY = (int)(relY * height);

            if (texX < 0 || texY < 0 || texX >= width || texY >= height)
                return collisionPos;

            float alpha = solidMap[texX, texY];

            collisionPos[0] = worldX;
            collisionPos[1] = worldY;
            collisionPos[2] = alpha;

            return collisionPos;
        }
    }
}
