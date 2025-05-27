using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceManager : MonoBehaviour
{
    public GameObject resourcePrefab;
    public float spawnIntervals = 3f;
    public int maxResources = 20;
    public Vector2 spawnAreaMin = new Vector2(-10, -10);
    public Vector2 spawnAreaMax = new Vector2(10, 10);
    
    private List<ResourceNode> activeResources = new List<ResourceNode>();

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnIntervals);

            if (activeResources.Count < maxResources)
                SpawnResource();
        }
    }

    private void SpawnResource()
    {
        Vector3 pos = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            0,
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );
        
        GameObject res = Instantiate(resourcePrefab, pos, Quaternion.identity);
        ResourceNode node = res.GetComponent<ResourceNode>();
        activeResources.Add(node);
    }

    public ResourceNode GetClosestFreeResource(Vector3 position)
    {
        float minDist = float.MaxValue;
        ResourceNode closest = null;

        foreach (var res in activeResources)
        {
            if (res == null || res.IsClaimed) continue;
            
            float dist = Vector3.Distance(position, res.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = res;
            }
        }
        return closest;
    }
    
}
