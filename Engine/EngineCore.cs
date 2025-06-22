namespace Engine {
    public static class EngineCore {
        private static bool _engineState = false;

        public static void Initialize(string defaultVert, string defaultFrag, string defaultTexture,
                bool debugOutput) {
            if (debugOutput == true) {
                ErrorChecker.InitializeGLDebugCallback();
                Console.WriteLine("Enabe OpenGL Debug Print");
            }
            ResourceManager.LoadShader("default", defaultVert, defaultFrag);
            Console.WriteLine("Init default shader \nVertPath: " + defaultVert +
                    "\nFragPaht: " + defaultFrag);
            ResourceManager.LoadTexture("default", defaultTexture);
            Console.WriteLine("Init default texture\nTexturePaht: " + 
                    defaultTexture);
            // Enable Debuggouput Console

            _engineState = true;
        }

        public static bool GetEngineState() {
            return _engineState;
        }
    }
}
