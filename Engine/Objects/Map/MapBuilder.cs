namespace Engine {
    public static class MapBuilder{
        private static Dictionary<string, CollisionMap> _maps = new();

        public static void Initialize(string[] maps){
            foreach (string map in maps) {
                if (!_maps.ContainsKey(map)) {
                    Console.WriteLine("Generating Collision Map: " + map);
                    _maps[map] = new CollisionMap(map);
                }
            }
        }

        public static float[] CheckCollision(string map, float mapPosX, float mapPosY, float carPosX, float carPoxY){
            float[] colissionPos;
            return colissionPos;
    }
}
