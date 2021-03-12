using UnityEngine;
using System.Runtime.InteropServices;

public class Struct : MonoBehaviour
{
    public struct Particle
    {
        public Vector2 position;
        public Vector2 velocity;
        public float   radius;
    }

    public Material material;
    public int      count = 30;
    public float    speed = 0.002f;

    ComputeBuffer particleBuffer;
    Particle[]    particles;

    void Start()
    {
        particleBuffer = new ComputeBuffer(count, Marshal.SizeOf(typeof(Particle)));

        particles = new Particle[count];

        for (int i = 0; i < count; i++)
        {
            particles[i] = new Particle()
            {
                position = new Vector2(Random.value, Random.value),
                velocity = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * speed,
                radius   = Random.value * 0.1f,
            };
        }

        particleBuffer.SetData(particles);

        material.SetBuffer("_ParticleBuffer", particleBuffer);
    }

    void Update()
    {
        Particle particle;

        for (int i = 0; i < count; i++)
        {
            particle = particles[i];

            particle.position += particle.velocity;

            if (particle.position.x <= 0 || 1 <= particle.position.x)
            {
                particle.velocity.x *= -1;
            }

            if (particle.position.y <= 0 || 1 <= particle.position.y)
            {
                particle.velocity.y *= -1;
            }

            particles[i] = particle;
        }

        particleBuffer.SetData(particles);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, material);
    }

    void OnDestroy()
    {
        particleBuffer.Dispose();
    }
}