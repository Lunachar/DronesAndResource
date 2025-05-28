using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseSpawner : MonoBehaviour
{
    public enum Faction {Red, Blue};

    [SerializeField] public Faction baseFaction;
    [SerializeField] public Material redMaterial;
    [SerializeField] public Material blueMaterial;
    
    public GameObject dronePrefab;
    public int numberOfDrones = 1;
    public float spawnInterval = 1f;

    private void Start()
    {
        AssignMaterial();
        StartCoroutine(SpawnDronesRoutine());
    }

    private void AssignMaterial()
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        Material instancedMaterial = null;

        if (baseFaction == Faction.Red && redMaterial != null)
            instancedMaterial = new Material(redMaterial);
        else if (baseFaction == Faction.Blue && blueMaterial != null)
            instancedMaterial = new Material(blueMaterial);
        
        if (instancedMaterial != null)
            renderer.material = instancedMaterial;
    }

    private IEnumerator SpawnDronesRoutine()
    {
        for (int i = 0; i < numberOfDrones; i++)
        {
            SpawnDrone();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnDrone()
    {
        Vector3 spawnPos = transform.position + Vector3.forward * 2f + Random.insideUnitSphere * 1.5f;
        spawnPos.y = 0;
        
        GameObject drone = Instantiate(dronePrefab, spawnPos, Quaternion.identity);
        var droneMaterial = drone.GetComponentInChildren<Renderer>();
        
        droneMaterial.material = baseFaction == Faction.Red ? redMaterial : blueMaterial;
        
        DroneAI ai = drone.GetComponent<DroneAI>();
        ai.SetFaction(baseFaction == Faction.Red ? DroneAI.Faction.Red : DroneAI.Faction.Blue);
    }
}
