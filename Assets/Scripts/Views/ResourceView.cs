using UnityEngine;

public class ResourceView : ViewBase
{
    [SerializeField] private ParticleSystem spawnEffect;
    [SerializeField] private ParticleSystem collectEffect;
    
    private ResourceData resourceData;
    
    public void Initialize(ResourceData data)
    {
        resourceData = data;
        transform.position = data.Position;
        PlaySpawnEffect();
    }
    
    public void OnCollectionStarted()
    {
        PlayCollectEffect();
    }
    
    public void OnCollectionFinished()
    {
        gameObject.SetActive(false);
    }
    
    private void PlaySpawnEffect()
    {
        if (spawnEffect != null)
        {
            spawnEffect.Play();
        }
    }
    
    private void PlayCollectEffect()
    {
        if (collectEffect != null)
        {
            collectEffect.Play();
        }
    }
} 