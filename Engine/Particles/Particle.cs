using OpenTK.Mathematics;

namespace Engine {
    public class Particle {
        public Vector4 particlePos = new Vector4(.0f, .0f, .0f, .0f);
        public float velocity = 0f;
        public float scale = 1f;
        public float life = 0f;

        public Particle(){
        }
    }
}
