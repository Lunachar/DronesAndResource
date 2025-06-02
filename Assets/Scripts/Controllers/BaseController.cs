using UnityEngine;
using System.Collections.Generic;

public class BaseController : MonoBehaviour
{
    [SerializeField] private Transform droneSpawnPoint;
    [SerializeField] private float spawnRadius = 2f;
    
    private BaseView baseView;
    private BaseData baseData;
    private ObjectPool<DroneView> dronePool;
    private List<DroneController> activeDrones;
    
    private void Awake()
    {
        baseView = GetComponent<BaseView>();
        activeDrones = new List<DroneController>();
    }
    
    public void Initialize(BaseData data, ObjectPool<DroneView> dronePool)
    {
        baseData = data;
        this.dronePool = dronePool;
        baseView.Initialize(data);
    }
    
    public void UpdateDroneCount(int newCount)
    {
        while (activeDrones.Count > newCount)
        {
            RemoveDrone();
        }
        
        while (activeDrones.Count < newCount)
        {
            SpawnDrone();
        }
        
        baseData.DroneCount = newCount;
        GameEvents.TriggerDroneCountChanged(baseData);
    }
    
    private void SpawnDrone()
    {
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0;
        Vector3 spawnPosition = droneSpawnPoint.position + randomOffset;
        
        DroneView droneView = dronePool.Get();
        droneView.transform.position = spawnPosition;
        droneView.Initialize(baseData.Faction);
        
        DroneController droneController = droneView.GetComponent<DroneController>();
        droneController.Initialize(baseData);
        
        activeDrones.Add(droneController);
    }
    
    private void RemoveDrone()
    {
        if (activeDrones.Count == 0) return;
        
        DroneController drone = activeDrones[activeDrones.Count - 1];
        activeDrones.RemoveAt(activeDrones.Count - 1);
        
        drone.GetComponent<DroneView>().Destroy();
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public IEnumerable<DroneController> GetActiveDrones()
    {
        throw new System.NotImplementedException();
    }
} 