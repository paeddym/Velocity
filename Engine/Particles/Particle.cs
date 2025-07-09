using OpenTK.Mathematics;

namespace Engine {
    public class Particle {
        public Vector2 particlePos = new Vector2(.0f, .0f);
        public float velocity = 0f;
        public float scale = 1f;
        public float life = 0f;

        public Particle(){
        }
    }
}
