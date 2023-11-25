using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WindParticleSettings : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    public float speed = 5.0f;

    public float minTrailLength = 0.25f;
    public float maxTrailLength = 0.5f;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }


    public void SetWindLength(int length)
    {
        float lifetime = (float)length / speed;

        float minTrailLifeTime = minTrailLength / (float)lifetime;
        float maxTrailLifeTime = maxTrailLength / (float)lifetime;

        _particleSystem.startLifetime = lifetime;

        ParticleSystem.TrailModule trailData = _particleSystem.trails;
        trailData.lifetime = new ParticleSystem.MinMaxCurve(minTrailLifeTime, maxTrailLifeTime);
    }
}
