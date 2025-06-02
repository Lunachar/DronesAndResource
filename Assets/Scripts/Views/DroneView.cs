using UnityEngine;
using System;

public class DroneView : ViewBase
{
    [SerializeField] private ParticleSystem spawnEffect;
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private ParticleSystem deliveryEffect;
    
    private Faction faction;
    private ResourceData currentTarget;
    private bool isCollecting;
    private float collectionStartTime;
    
    public Faction Faction => faction;
    public ResourceData CurrentTarget => currentTarget;
    public bool IsCollecting => isCollecting;
    
    public event Action<DroneView> OnDroneDestroyed;
    
    public void Initialize(Faction faction)
    {
        this.faction = faction;
        PlaySpawnEffect();
    }
    
    public void SetTarget(ResourceData target)
    {
        currentTarget = target;
    }
    
    public void StartCollecting()
    {
        isCollecting = true;
        collectionStartTime = Time.time;
        PlayCollectEffect();
        GameEvents.TriggerDroneStartedCollecting(this, currentTarget);
    }
    
    public void FinishCollecting()
    {
        isCollecting = false;
        currentTarget = null;
        PlayDeliveryEffect();
        GameEvents.TriggerDroneFinishedCollecting(this, currentTarget);
    }
    
    public bool HasCollectedLongEnough()
    {
        return isCollecting && Time.time - collectionStartTime >= 2f;
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
    
    private void PlayDeliveryEffect()
    {
        if (deliveryEffect != null)
        {
            deliveryEffect.Play();
        }
    }
    
    public void Destroy()
    {
        OnDroneDestroyed?.Invoke(this);
        gameObject.SetActive(false);
    }
} 