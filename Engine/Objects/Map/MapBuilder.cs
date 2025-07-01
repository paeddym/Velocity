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

        public static float[] CheckCollision(string map, float carPosX, float carPoxY){
            CollisionMap collisionMap = GetCollisionMap(map);
            GameObject ?mapObj = ObjectManager.GetGameObject(map);
            float[] collision = {0f, 0f, 0f};
            collision = collisionMap.IsSolid(carPosX, carPoxY, mapObj.objectPos.X, 
                    mapObj.objectPos.Y, 
                    mapObj.scale, 
                    mapObj.scale);

            return collision;
        }

        public static CollisionMap GetCollisionMap(string map){
            return _maps[map];
        }
    }
}
