using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineTris
{
    public class ParticleEngine
    {

        private Random random;
        private List<Particle> particles;
        private List<Texture2D> textures;

        public Vector2 EmitterLocation { get; set; }
        public bool Active { get; set; }

        public int EffectNumber;


        public ParticleEngine(List<Texture2D> textures, Vector2 location, int effectNumber)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();

            EffectNumber = effectNumber;
        }


        private Particle GenerateNewParticle()
        {

            if (EffectNumber == 1)
            {
                Texture2D texture = textures[random.Next(textures.Count)];
                Vector2 position = EmitterLocation;
                Vector2 velocity = new Vector2(((float)Math.Cos(1f * (float)(random.NextDouble() * .5f - 1)) * -.3f), ((float)Math.Cos(1f * (float)(random.NextDouble() * .5f - 1))) * -2f);
                float angle = 0;
                float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                float size = (float)random.NextDouble() * 4f;
                int timetolive = 2;

                return new Particle(texture, position, velocity, angle, angularVelocity, color, size, timetolive);
            }

            else if (EffectNumber == 2)
            {
                float num1 = random.Next(-1, 1);
                float num2 = random.Next(-1, 1);

                Texture2D texture = textures[random.Next(textures.Count)];
                Vector2 position = EmitterLocation;
                Vector2 velocity = new Vector2(num1, num2); //new Vector2(1f * (float)(random.NextDouble() * 2 - 1), 1f * (float)(random.NextDouble() * 2 - 1));
                float angle = 0;
                float angularVelocity = 0.3f * (float)(random.NextDouble() * 2 - 1);
                Color color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                float size = (float)random.NextDouble() * 1.7f;
                int timetolive = 2 + random.Next(5);

                return new Particle(texture, position, velocity, angle, angularVelocity, color, size, timetolive);
            }
            return null;
        }


        public void Update(int total)                   // include the number of particles to be generated
        {
            int Total = total;

            if (Active == true)                         // if the particle system is Active; generate new particle
            {
                for (int i = 0; i < Total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }

                Active = false;
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TimeToLive <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}
