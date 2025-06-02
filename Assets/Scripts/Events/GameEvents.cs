using UnityEngine;
using System;

public static class GameEvents
{
    // Resource events
    public static event Action<ResourceData> OnResourceSpawned;
    public static event Action<ResourceData> OnResourceCollected;
    public static event Action<ResourceData> OnResourceCollectionStarted;
    
    // Drone events
    public static event Action<DroneView> OnDroneSpawned;
    public static event Action<DroneView> OnDroneDespawned;
    public static event Action<DroneView, ResourceData> OnDroneStartedCollecting;
    public static event Action<DroneView, ResourceData> OnDroneFinishedCollecting;
    
    // Base events
    public static event Action<BaseData> OnResourceDelivered;
    public static event Action<BaseData> OnDroneCountChanged;
    
    // Game state events
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    
    // Event triggers
    public static void TriggerResourceSpawned(ResourceData resource) => OnResourceSpawned?.Invoke(resource);
    public static void TriggerResourceCollected(ResourceData resource) => OnResourceCollected?.Invoke(resource);
    public static void TriggerResourceCollectionStarted(ResourceData resource) => OnResourceCollectionStarted?.Invoke(resource);
    
    public static void TriggerDroneSpawned(DroneView drone) => OnDroneSpawned?.Invoke(drone);
    public static void TriggerDroneDespawned(DroneView drone) => OnDroneDespawned?.Invoke(drone);
    public static void TriggerDroneStartedCollecting(DroneView drone, ResourceData resource) => OnDroneStartedCollecting?.Invoke(drone, resource);
    public static void TriggerDroneFinishedCollecting(DroneView drone, ResourceData resource) => OnDroneFinishedCollecting?.Invoke(drone, resource);
    
    public static void TriggerResourceDelivered(BaseData baseData) => OnResourceDelivered?.Invoke(baseData);
    public static void TriggerDroneCountChanged(BaseData baseData) => OnDroneCountChanged?.Invoke(baseData);
    
    public static void TriggerGameStarted() => OnGameStarted?.Invoke();
    public static void TriggerGamePaused() => OnGamePaused?.Invoke();
    public static void TriggerGameResumed() => OnGameResumed?.Invoke();
} 