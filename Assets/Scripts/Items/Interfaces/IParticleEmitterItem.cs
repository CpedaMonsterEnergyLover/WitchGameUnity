using UnityEngine;

public interface IParticleEmitterItem
{
    public bool HasParticles { get; }
    public ParticleSystem ParticleSystem { get; }
    public ItemParticleEmissionMode EmissionMode { get; }
}
