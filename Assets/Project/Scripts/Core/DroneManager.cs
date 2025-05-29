using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class DroneManager : MonoBehaviour
{
    public static DroneManager Instance;

    public GameObject dronePrefab;
    public int maxDrones = 10;
    public List<Drone> dronePool = new();
    public List<ResourceNode> resourceNodes;


    [SerializeField] private int currentDroneCount = 0;
    [SerializeField] private ResourceManager resourceManager;
    
    private Dictionary<Base.Faction, Base> bases = new();
    public int numberOfFaction =2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    

    public void RegisterBase(Base baseComponent)
    {
        if (!bases.ContainsKey(baseComponent.faction))
        {
            bases[baseComponent.faction] = baseComponent;
            Debug.Log($"[DroneManager] Зарегистрирована база для фракции {baseComponent.faction}");
        }
        else
            Debug.LogWarning($"Base for faction {baseComponent.faction} already registered!");
    }

    public Transform GetBaseTransform(Base.Faction faction)
    {
        if (bases.TryGetValue(faction, out Base baseObj))
            return baseObj.transform;
        Debug.LogError($"No base found for faction {faction}");
        return null;
    }

    public Base GetBase(Base.Faction faction)
    {
        Base b;
        if (bases.TryGetValue(faction, out b))
        {
            return b;
        }
        return null;
    }

    public void InitializeDrones()
    {
        int dronesPerFaction = maxDrones / numberOfFaction;
        dronePool.Clear();

        foreach (Base.Faction faction in Enum.GetValues(typeof(Base.Faction)))
        {
            var baseObj = GetBase(faction);
            var baseSpawnPos = baseObj.transform.position;
            for (int i = 0; i < dronesPerFaction; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                Vector3 spawnPos = baseSpawnPos + offset;

                GameObject obj = Instantiate(dronePrefab, spawnPos, Quaternion.identity);

                Drone drone = obj.GetComponent<Drone>();
                if (drone == null)
                {
                    Debug.LogError("No Drone script found on spawned object");
                    Destroy(obj);
                    continue;
                }
                
                drone.faction = faction;
                drone.Initialize(faction, baseObj.GetMaterial());

                obj.SetActive(false);
                dronePool.Add(drone);
            }
        }
    }

    public void SetDroneCount(int countPerFaction)
    {
        currentDroneCount = Mathf.Clamp(countPerFaction, 0, maxDrones);

        Dictionary<Base.Faction, int> factionCounts = new()
        {
            { Base.Faction.Red, 0 },
            { Base.Faction.Blue, 0 }
        };

        foreach (var drone in dronePool)
        {
            var f = drone.faction;

            if (factionCounts[f] < currentDroneCount)
            {
                if (!drone.gameObject.activeSelf)
                {
                    Transform baseTransform = GetBaseTransform(f);
                    Vector3 spawnPos = baseTransform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                    drone.transform.position = spawnPos;
                    drone.GetComponent<NavMeshAgent>().Warp(spawnPos);
                }
                
                drone.gameObject.SetActive(true);
                StartCoroutine(StartDroneWhenResourcesReady(drone));
                factionCounts[f]++;
            }
            else
            {
                drone.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator StartDroneWhenResourcesReady(Drone drone)
    {
        while (!resourceManager.HasAvailableResources())
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        drone.StartWorking();
    }

    public void SetDroneSpeed(int value)
    {
        foreach (var drone in dronePool)
        {
            if (drone.gameObject.activeInHierarchy)
                drone.SetSpeed(value);
        }
    }


    public ResourceNode GetNearestAvailaleResource(Vector3 dronePos, Vector3 basePos)
    {
        return resourceManager.GetBestResource(dronePos, basePos);
    }

    public Material GetMaterial(Base.Faction faction)
    {
        var baseObj = GetBase(faction);
        if (baseObj != null)
            return baseObj.GetMaterial();
        
        return null;
    }

    public void UnregisterResource(ResourceNode resourceNode)
    {
        if(resourceNodes.Contains(resourceNode))
            resourceNodes.Remove(resourceNode);
    }
}