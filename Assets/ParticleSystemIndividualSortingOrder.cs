
using UnityEngine;

[ExecuteInEditMode]
public class ParticleSystemIndividualSortingOrder : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles;
    private GameObject[] _spritePool;
    private int _maxParticles;
    
    private void Start()
    {
        InitializeIfNeeded();
        RecreatePool();
    }

    public void RecreatePool()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
        
        _maxParticles = _particleSystem.main.maxParticles;

        _spritePool = new GameObject[_maxParticles];
        for (int i = 0; i < _maxParticles; i++)
        {
            GameObject particleGameObject = new GameObject();
            particleGameObject.transform.SetParent(transform);
            SpriteRenderer particleSpriteRenderer = particleGameObject.AddComponent<SpriteRenderer>();
            // particleSpriteRenderer.renderingLayerMask = LayerMask.NameToLayer("Default");
            particleSpriteRenderer.sprite = _particleSystem.textureSheetAnimation.GetSprite(0);
            _spritePool[i] = particleGameObject;
        }
    }

    private void Update()
    {
        InitializeIfNeeded();
        int numParticlesAlive = _particleSystem.GetParticles(_particles);
        for (int i = 0; i < _maxParticles; i++)
        {
            if (i < numParticlesAlive)
            {
                GameObject mGameObject = _spritePool[i];
                Transform mTransform = mGameObject.transform;
                mTransform.position = _particles[i].position;
                SpriteRenderer mSprite = mGameObject.GetComponent<SpriteRenderer>();
                mSprite.color = _particles[i].GetCurrentColor(_particleSystem);
                mTransform.localScale = _particles[i].GetCurrentSize3D(_particleSystem);
                _spritePool[i].SetActive(true);
            }
            else
            {
                _spritePool[i].SetActive(false);
            }
        }
    }
    
    void InitializeIfNeeded()
    {
        if (_particleSystem == null)
            _particleSystem = GetComponent<ParticleSystem>();

        if (_particles == null || _particles.Length < _particleSystem.main.maxParticles)
        {
            _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        }

    }
}
