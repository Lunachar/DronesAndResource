using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private DroneView dronePrefab;
    [SerializeField] private ResourceView resourcePrefab;
    
    [Header("Base Settings")]
    [SerializeField] private BaseController redBase;
    [SerializeField] private BaseController blueBase;
    [SerializeField] private Vector3 redBasePosition = new Vector3(-10f, 0f, 0f);
    [SerializeField] private Vector3 blueBasePosition = new Vector3(10f, 0f, 0f);
    
    [Header("Resource Settings")]
    [SerializeField] private float resourceSpawnInterval = 5f;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(20f, 20f);
    [SerializeField] private float spawnAreaOffset = 5f;
    
    [Header("UI References")]
    [SerializeField] private Slider droneCountSlider;
    [SerializeField] private TextMeshProUGUI redBaseResourceText;
    [SerializeField] private TextMeshProUGUI blueBaseResourceText;
    
    private ObjectPool<DroneView> dronePool;
    private ObjectPool<ResourceView> resourcePool;
    private List<ResourceData> activeResources;
    private float nextResourceSpawnTime;
    
    private BaseData redBaseData;
    private BaseData blueBaseData;
    
    private void Awake()
    {
        activeResources = new List<ResourceData>();
        
        // Initialize pools
        dronePool = new ObjectPool<DroneView>(dronePrefab, transform);
        resourcePool = new ObjectPool<ResourceView>(resourcePrefab, transform);
        
        // Initialize bases
        redBaseData = new BaseData(Faction.Red, redBasePosition);
        blueBaseData = new BaseData(Faction.Blue, blueBasePosition);
        
        redBase.Initialize(redBaseData, dronePool);
        blueBase.Initialize(blueBaseData, dronePool);
        
        // Setup UI
        droneCountSlider.onValueChanged.AddListener(OnDroneCountChanged);
    }
    
    private void Start()
    {
        GameEvents.TriggerGameStarted();
    }
    
    private void Update()
    {
        if (Time.time >= nextResourceSpawnTime)
        {
            SpawnResource();
            nextResourceSpawnTime = Time.time + resourceSpawnInterval;
        }
        
        UpdateDroneTargets();
    }
    
    private void SpawnResource()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        ResourceData resourceData = new ResourceData(spawnPosition);
        
        ResourceView resourceView = resourcePool.Get();
        resourceView.Initialize(resourceData);
        
        activeResources.Add(resourceData);
        GameEvents.TriggerResourceSpawned(resourceData);
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-spawnAreaSize.x/2, spawnAreaSize.x/2);
        float z = Random.Range(-spawnAreaSize.y/2, spawnAreaSize.y/2);
        
        // Ensure resources don't spawn too close to bases
        Vector3 position = new Vector3(x, 0f, z);
        if (Vector3.Distance(position, redBasePosition) < spawnAreaOffset ||
            Vector3.Distance(position, blueBasePosition) < spawnAreaOffset)
        {
            return GetRandomSpawnPosition();
        }
        
        return position;
    }
    
    private void UpdateDroneTargets()
    {
        foreach (ResourceData resource in activeResources.ToArray())
        {
            if (resource.IsBeingCollected) continue;
            
            DroneController nearestRedDrone = FindNearestAvailableDrone(resource, Faction.Red);
            DroneController nearestBlueDrone = FindNearestAvailableDrone(resource, Faction.Blue);
            
            if (nearestRedDrone != null && nearestBlueDrone != null)
            {
                float redDistance = Vector3.Distance(nearestRedDrone.transform.position, resource.Position);
                float blueDistance = Vector3.Distance(nearestBlueDrone.transform.position, resource.Position);
                
                if (redDistance < blueDistance)
                {
                    nearestRedDrone.SetTarget(resource);
                }
                else
                {
                    nearestBlueDrone.SetTarget(resource);
                }
            }
            else if (nearestRedDrone != null)
            {
                nearestRedDrone.SetTarget(resource);
            }
            else if (nearestBlueDrone != null)
            {
                nearestBlueDrone.SetTarget(resource);
            }
        }
    }
    
    private DroneController FindNearestAvailableDrone(ResourceData resource, Faction faction)
    {
        DroneController nearestDrone = null;
        float nearestDistance = float.MaxValue;
        
        BaseController baseController = faction == Faction.Red ? redBase : blueBase;
        foreach (DroneController drone in baseController.GetActiveDrones())
        {
            if (drone.CanCollectResource(resource))
            {
                float distance = Vector3.Distance(drone.transform.position, resource.Position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestDrone = drone;
                }
            }
        }
        
        return nearestDrone;
    }
    
    private void OnDroneCountChanged(float value)
    {
        int count = Mathf.RoundToInt(value);
        redBase.UpdateDroneCount(count);
        blueBase.UpdateDroneCount(count);
    }
    
    private void OnResourceCollected(ResourceData resource)
    {
        activeResources.Remove(resource);
        GameEvents.TriggerResourceCollected(resource);
    }
    
    private void OnResourceCollectionStarted(ResourceData resource)
    {
        resource.IsBeingCollected = true;
        resource.CollectionStartTime = Time.time;
        GameEvents.TriggerResourceCollectionStarted(resource);
    }

    public void SetResourceSpawnInterval(float value)
    {
        throw new System.NotImplementedException();
    }
} 