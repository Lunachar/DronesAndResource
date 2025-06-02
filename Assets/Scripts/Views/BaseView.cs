using UnityEngine;
using TMPro;

public abstract class ViewBase : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        SubscribeToEvents();
    }
    
    protected virtual void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    
    protected virtual void SubscribeToEvents() { }
    protected virtual void UnsubscribeFromEvents() { }
    
    public virtual void Initialize() { }
    public virtual void Cleanup() { }
}

public class BaseView : ViewBase
{
    [SerializeField] private TextMeshProUGUI resourceCountText;
    [SerializeField] private TextMeshProUGUI droneCountText;
    [SerializeField] private ParticleSystem resourceDeliveryEffect;
    
    private BaseData baseData;
    
    protected override void SubscribeToEvents()
    {
        GameEvents.OnResourceDelivered += OnResourceDelivered;
        GameEvents.OnDroneCountChanged += OnDroneCountChanged;
    }
    
    protected override void UnsubscribeFromEvents()
    {
        GameEvents.OnResourceDelivered -= OnResourceDelivered;
        GameEvents.OnDroneCountChanged -= OnDroneCountChanged;
    }
    
    public void Initialize(BaseData data)
    {
        baseData = data;
        UpdateUI();
    }
    
    private void OnResourceDelivered(BaseData deliveredBase)
    {
        if (deliveredBase == baseData)
        {
            PlayDeliveryEffect();
            UpdateUI();
        }
    }
    
    private void OnDroneCountChanged(BaseData changedBase)
    {
        if (changedBase == baseData)
        {
            UpdateUI();
        }
    }
    
    private void UpdateUI()
    {
        if (resourceCountText != null)
        {
            resourceCountText.text = $"Resources: {baseData.CollectedResources}";
        }
        
        if (droneCountText != null)
        {
            droneCountText.text = $"Drones: {baseData.DroneCount}";
        }
    }
    
    private void PlayDeliveryEffect()
    {
        if (resourceDeliveryEffect != null)
        {
            resourceDeliveryEffect.Play();
        }
    }
} 