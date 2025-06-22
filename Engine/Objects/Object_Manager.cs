// The idee is that the ObjectManger will use the information stored inside of an GameObject
// To set everything up in the OpenGL context so the GameObject can be drawn.
// There is list with all GameObjects, as all objects have a name you can reverenze them with a
// name probaply something like this ObjectManager objects;
// Game object = objects.GetGameObject("<ObjectName>");
// object.UpdatePosition(x, y, z, r);

namespace Engine {
    public static class ObjectManager {
        private static List<GameObject> _gameObjects = new List<GameObject>();

        public static void AddGameObject(GameObject gameObject) {
            _gameObjects.Add(gameObject);
        }

        public static GameObject? GetGameObject(String gameObjectName) {
            return _gameObjects.FirstOrDefault(obj => obj.objectName == gameObjectName);
        }

        public static void DeleteGameObject(String gameObjectName) {
            _gameObjects.RemoveAll(obj => obj.objectName == gameObjectName);
        }

        public static void DrawAll() {
            foreach (var gameObject in _gameObjects) {
                gameObject.Draw();
            }
        }
    }
}

