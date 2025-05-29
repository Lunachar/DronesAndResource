using System;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] public GameObject basePrefab;
    [SerializeField] public Material redMaterial;
    [SerializeField] public Material blueMaterial;
    [SerializeField] private NavMeshSurface surface;

    private void Start()
    {
        SpawnBase(Base.Faction.Red);
        SpawnBase(Base.Faction.Blue);
        
        surface.BuildNavMesh();
        
        DroneManager.Instance.InitializeDrones();
        DroneManager.Instance.SetDroneCount(1);
    }

    private void SpawnBase(Base.Faction faction)
    {
        Vector3 spawnPos = GetRandomPosition();
        GameObject baseGO = Instantiate(basePrefab, spawnPos, Quaternion.identity);

        var baseComponent = baseGO.GetComponent<Base>();
        baseComponent.faction = faction;
        baseComponent.material = faction == Base.Faction.Red ? redMaterial : blueMaterial;
        baseComponent.GetComponent<Base>().ApplyMaterial();

        DroneManager.Instance.RegisterBase(baseComponent);
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-10f, 10f),
            0.5f,
            Random.Range(-10f, 10f)
        );
    }
    //
    // private void AssignMaterial()
    // {
    //     var renderer = GetComponentInChildren<Renderer>();
    //     if (renderer == null) return;
    //
    //     Material instancedMaterial = null;
    //
    //     if (baseFaction == Faction.Red && redMaterial != null)
    //         instancedMaterial = new Material(redMaterial);
    //     else if (baseFaction == Faction.Blue && blueMaterial != null)
    //         instancedMaterial = new Material(blueMaterial);
    //     
    //     if (instancedMaterial != null)
    //         renderer.material = instancedMaterial;
    // }
    //
    // public Material GetAssignedMaterial()
    // {
    //     if (baseFaction == Faction.Red && redMaterial != null)
    //         return new Material(redMaterial);
    //     else if (baseFaction == Faction.Blue && blueMaterial != null)
    //         return new Material(blueMaterial);
    //     return null;
    // }
    // public static Transform GetBaseTransform(Faction faction)
    // { 
    //     switch (faction)
    //     {
    //         case Faction.Red:
    //             return redBaseTransform != null ? redBaseTransform : throw new Exception("Red base transform is not assigned!");
    //         case Faction.Blue:
    //             return blueBaseTransform != null ? blueBaseTransform : throw new Exception("Blue base transform is not assigned!");
    //         default:
    //             throw new Exception("Unknown faction!");
    //     }
    // }
}
