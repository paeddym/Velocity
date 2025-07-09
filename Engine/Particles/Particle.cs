using OpenTK.Mathematics;

namespace Engine{
    public class Particle {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Lifetime;
        public float Age;
        public float Size;

        private GameObject _obj;

        public bool IsAlive => Age < Lifetime;

        public Particle(Vector2 position, Vector2 velocity, float lifetime, Texture texture) {
            Position = position;
            Velocity = velocity;
            Lifetime = lifetime;
            Age = 0f;
            Size = 1f;

            _obj = new GameObject("particle", "particleTex");
        }

        public void Update(float dt) {
            Age += dt;
            Position += Velocity;

            float lifeRatio = 1.0f - (Age / Lifetime);
            Size = 0.5f + (1.0f - lifeRatio) * 1.5f;

            _obj.objectPos.X = Position.X;
            _obj.objectPos.Y = Position.Y;
            _obj.objectPos.Z = .1f;
            _obj.scale = Size;
        }

        public void Draw() {
            _obj.Draw();
        }
    }
}

