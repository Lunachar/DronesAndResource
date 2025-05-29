using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Drone Settings")]
    public Slider droneCountSlider;
    public TextMeshProUGUI droneCountText;
    
    public Slider droneSpeedSlider;
    public TextMeshProUGUI droneSpeedText;
    
    [Header("Resource Settings")]
    public TMP_InputField resourceSpawnRateField;
    
    [Header("Path Toggle")]
    public Toggle pathToggle;

    [Header("Stats")]
    public TextMeshProUGUI blueCounterText;
    public TextMeshProUGUI redCounterText;

    private void Start()
    {
        droneCountSlider.onValueChanged.AddListener(OnDroneCountSliderChanged);
        droneSpeedSlider.onValueChanged.AddListener(OnDroneSpeedSliderChanged);
        resourceSpawnRateField.onEndEdit.AddListener(OnResourceSpawnRateFieldChanged);
        pathToggle.onValueChanged.AddListener(OnTogglePath);
        
        UpdateLabels();
    }


    private void OnDroneCountSliderChanged(float value)
    {
        droneCountText.text = $"Дроны на \n фракцию: {value}";
        DroneManager.Instance.SetDroneCount((int)value);
    }

    private void OnDroneSpeedSliderChanged(float value)
    {
        droneSpeedText.text = $"Скорость \n дронов: {value}";
        DroneManager.Instance.SetDroneSpeed((int)value);
    }

    private void OnResourceSpawnRateFieldChanged(string input)
    {
        if(int.TryParse(input, out int value))
        {
            GameManager.Instance.SetResourceSpawnRate(value);
            resourceSpawnRateField.text = value.ToString("0");
        }
    }

    private void OnTogglePath(bool isOn)
    {
        GameManager.Instance.SetPathRendering(isOn);
    }
    private void UpdateLabels()
    {
        OnDroneCountSliderChanged(droneCountSlider.value);
        OnDroneSpeedSliderChanged(droneSpeedSlider.value);
        resourceSpawnRateField.text = GameManager.Instance.GetResourceManager().spawnIntervals.ToString("0");
    }

    public void UpdateCounter(int blueCounter, int redCounter)
    {
        blueCounterText.text = $"Синий: {blueCounter}";
        redCounterText.text = $"Красный: {redCounter}";
    }
}
