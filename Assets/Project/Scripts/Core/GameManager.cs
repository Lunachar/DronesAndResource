using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int droneCountPerFaction = 3;
    public float droneSpeed = 5f;
    public float resourceSpawnRate = 2f;
    public bool showPaths = true;

    public UIManager ui;

    private int blueScore = 0;
    private int redScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // предотвращает дубликаты
            return;
        }
        Instance = this;
    }

    public void SetDroneCountPerFaction(int value)
    {
        droneCountPerFaction = value;
    }

    public void SetDroneSpeed(int value)
    {
        droneSpeed = value;
        foreach (var drone in FindObjectsOfType<DroneAI>())
        {
            drone.SetSpeed(value);
        }
    }

    public void SetResourceSpawnRate(int value)
    {
        resourceSpawnRate = value;
    }

    public void SetPathRendering(bool value)
    {
        showPaths = value;
        foreach (var drone in FindObjectsOfType<DroneAI>())
        {
            drone.TogglePathRendering(value);
        }
    }

    public void AddResourceCollected(DroneAI.Faction faction)
    {
        if(faction == DroneAI.Faction.Blue)
            blueScore++;
        else
            redScore++;
        
        ui.UpdateCounter(blueScore, redScore);
    }
}
