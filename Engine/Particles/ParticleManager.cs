using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine{
    public static class ParticleManager {
        private static Random rand = new Random();
        private static List<Particle> particles = new();
        private static Texture texture;

        private const int MAX_PARTICLES = 30;

        public static void Initialize() {
            texture = ResourceManager.GetTexture("particleTex");
        }

        public static void Emit(GameObject emitter, float speed) {
            if (particles.Count >= MAX_PARTICLES)
                return;

            Vector2 pos = new Vector2(emitter.objectPos.X - emitter.front.X * .5f + ((float)rand.NextDouble() - 0.5f) * 0.5f, 
                                      emitter.objectPos.Y - emitter.front.Y * .5f + ((float)rand.NextDouble() - 0.5f) * 0.5f);
            Vector2 vel = new Vector2(emitter.front.X, emitter.front.Y) * speed * 0.5f;
            float life = 1.0f + (float)Random.Shared.NextDouble();

            particles.Add(new Particle(pos, vel, life, texture));
        }

        public static void Update(float deltaTime) {
            for (int i = particles.Count - 1; i >= 0; i--) {
                particles[i].Update(deltaTime);
                if (!particles[i].IsAlive)
                    particles.RemoveAt(i);
            }
        }
        public static void Draw() {
            // Enable alpha blending
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Enable depth test but disable depth writing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(false);

            foreach (var p in particles) {
                p.Draw();
            }

            // Restore state
            GL.DepthMask(true);
        }
    }
}
