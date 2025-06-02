using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : ViewBase
{
    [Header("UI Elements")]
    [SerializeField] private Slider droneCountSlider;
    [SerializeField] private Slider resourceSpawnIntervalSlider;
    [SerializeField] private TextMeshProUGUI redBaseResourceText;
    [SerializeField] private TextMeshProUGUI blueBaseResourceText;
    [SerializeField] private TextMeshProUGUI redBaseDroneCountText;
    [SerializeField] private TextMeshProUGUI blueBaseDroneCountText;
    
    [Header("Settings")]
    [SerializeField] private float minResourceSpawnInterval = 1f;
    [SerializeField] private float maxResourceSpawnInterval = 10f;
    
    private GameManager gameManager;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        // Setup sliders
        droneCountSlider.minValue = 0;
        droneCountSlider.maxValue = 10;
        droneCountSlider.value = 1;
        
        resourceSpawnIntervalSlider.minValue = minResourceSpawnInterval;
        resourceSpawnIntervalSlider.maxValue = maxResourceSpawnInterval;
        resourceSpawnIntervalSlider.value = 5f;
    }
    
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
    
    private void OnResourceDelivered(BaseData baseData)
    {
        UpdateResourceText(baseData);
    }
    
    private void OnDroneCountChanged(BaseData baseData)
    {
        UpdateDroneCountText(baseData);
    }
    
    private void UpdateResourceText(BaseData baseData)
    {
        if (baseData.Faction == Faction.Red)
        {
            redBaseResourceText.text = $"Resources: {baseData.CollectedResources}";
        }
        else
        {
            blueBaseResourceText.text = $"Resources: {baseData.CollectedResources}";
        }
    }
    
    private void UpdateDroneCountText(BaseData baseData)
    {
        if (baseData.Faction == Faction.Red)
        {
            redBaseDroneCountText.text = $"Drones: {baseData.DroneCount}";
        }
        else
        {
            blueBaseDroneCountText.text = $"Drones: {baseData.DroneCount}";
        }
    }
    
    public void OnResourceSpawnIntervalChanged(float value)
    {
        gameManager.SetResourceSpawnInterval(value);
    }
} 