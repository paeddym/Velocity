namespace Engine {
    // The ResourceManager is used to load and store textures and shaders as a 
    // <string, Resource> key value pare in a Dict. So it is more easy to keep track of
    // them and meke them more reusable.
    public static class ResourceManager {
        private static Dictionary<string, Shader> _shaders = new();
        private static Dictionary<string, Texture> _textures = new();
        
        // These methods are used to create shaders and retreve them after they are
        // added to the Dict, so it should be more easy to reuse them in the future
        public static Shader LoadShader(string name, string vertPath, string fragPath) {
            if (!_shaders.ContainsKey(name)) {
                Console.WriteLine("Building Shader: " + name);
                _shaders[name] = new Shader(vertPath, fragPath);
            }
            return _shaders[name];
        }

        public static Shader GetShader(string name) {
            return _shaders[name];
        }
        
        // These methods are used to create shaders and retreve them after they are
        // added to the Dict, so it should be more easy to reuse them in the future
        public static Texture LoadTexture(string name, string texturePath) {
            if (!_textures.ContainsKey(name)) {
                _textures[name] = new Texture(texturePath);
            }
            return _textures[name];
        }

        public static Texture GetTexture(string name) {
            return _textures[name];
        }
    }
}
