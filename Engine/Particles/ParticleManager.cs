using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine {
    public class ParticleManager {
        private List<Particle> _particles = new List<Particle>();
        private Texture _texture;
        private Shader _shader;
        private int _particleAmount;

        private int VAO = GL.GenVertexArray();

        private int lastUsedParticle = 0;

        public ParticleManager(string texture, string shader, int amoutn){
            this._texture = ResourceManager.GetTexture(texture);
            this._shader = ResourceManager.GetShader(shader);
            this._particleAmount = amoutn;

            int VBO = GL.GenBuffer();
            float[] particleQuad = {
                0.0f, 1.0f, 0.0f, 1.0f,
                1.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 0.0f,

                0.0f, 1.0f, 0.0f, 1.0f,
                1.0f, 1.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 1.0f, 0.0f
            };
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, particleQuad.Length *  sizeof(float), particleQuad, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            for (uint i = 0; i < amoutn; i++) {
                _particles.Add(new Particle());
            }
        }

        public void Draw() {
            this._shader.Use();

            Shapes.BindQuad();

            //Console.WriteLine("[PARTICLE]: Draw particle");
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0.0f, 800.0f, 0.0f, 600.0f, -1.0f, 1.0f);
            int projectionLocation =  GL.GetUniformLocation(_shader.Handle, "projection");
            GL.UniformMatrix4(projectionLocation, false, ref projection);

            int offset = GL.GetUniformLocation(_shader.Handle, "model");

            foreach (Particle p in _particles) {
                if (p.life > 0.0f) {
                    GL.ActiveTexture(TextureUnit.Texture0);
                    this._texture.Use();

                    int modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
                    Matrix4 model = Matrix4.CreateScale(p.scale) * 
                        Matrix4.CreateTranslation(p.particlePos.X, p.particlePos.Y, 0.0f);

                    GL.UniformMatrix4(modelLocation, false, ref model);

                    GL.DrawElements(PrimitiveType.Triangles, Shapes.GetQuadIndices(), DrawElementsType.UnsignedInt, 0);
                }
            }
        }

        public void Update(GameObject gameObject, int newParticles) {
            FrameEventArgs _event = InputProvider.GetFrameEvent();
            float _deltaTime = (float)_event.Time;
            //Console.WriteLine("[PARTICLE]: Update Particle");
            for (int i = 0; i < newParticles; i++) {
                int unusedParticle = firstUnusedParticle();
                respawnParticle(_particles[unusedParticle], gameObject);
            }

            for (int i = 0; i < _particleAmount; i++) {
                Particle p = _particles[i];
                p.life -= _deltaTime;
                if (p.life > 0.0f) {
                    //ToDo: Implement Update logic for particle
                }
            }
        }

        private int firstUnusedParticle() {
            for (int i = lastUsedParticle; i < _particleAmount; i++) {
                if (_particles[i].life <= 0.0f) {
                    lastUsedParticle = i;
                    return i;
                }
            }
            for (int i = 0; i < lastUsedParticle; i++) {
                if (_particles[i].life <= 0.0f) {
                    lastUsedParticle = i;
                    return i;
                }
            }
            lastUsedParticle = 0;
            return 0;
        }

        private void respawnParticle(Particle particle, GameObject gameObject) {
            particle.particlePos.X = gameObject.objectPos.X;
            particle.particlePos.Y = gameObject.objectPos.Y;
            particle.life = 10f;
        }
    }

    public static class testFuck {
        public static ParticleManager mm = new ParticleManager("carexaust", "particle", 100);
        public static void Init(){
        }
    }
}

