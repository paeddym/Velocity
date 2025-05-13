// The idee is that the ObjectManger will use the information stored inside of an GameObject
// To set everything up in the OpenGL context so the GameObject can be drawn.
// There is list with all GameObjects, as all objects have a name you can reverenze them with a
// name probaply something like this ObjectManager objects;
// Game object = objects.GetGameObject("<ObjectName>");
// object.UpdatePosition(x, y, z, r);

namespace Engine {
    public class ObjectManager {
        List<GameObject> _gameObjects = new List<GameObject>();

        public ObjectManager() {

        }

        public void AddGameObject(GameObject gameObject) {
            _gameObjects.Add(gameObject);
        }

        public void DeleteGameObject(String gameObjectName) {
            foreach (GameObject gameObject in _gameObjects) {
                if (gameObject.objectName == gameObjectName) {
                    _gameObjects.Remove(gameObject); 
                }
            }

        }

        public GameObject? GetGameObject(String gameObjectName) {
            foreach (GameObject gameObject in _gameObjects) {
                return gameObject;
            }
            return null;
        }
    }
}

