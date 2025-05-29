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

    public ResourceNode GetBestResource(Vector3 dronePos, Vector3 basePos)
    {
        ResourceNode bestNode = null;
        float bestScore = float.MaxValue;

        foreach (var node in activeResources)
        {
            if (node == null || !node.gameObject.activeInHierarchy || node.IsOccupied)
                continue;

            float distToDrone = Vector3.Distance(dronePos, node.transform.position);
            float distToBase = Vector3.Distance(node.transform.position, basePos);
            float score = distToDrone + distToBase * 0.5f;

            if (score < bestScore)
            {
                bestScore = score;
                bestNode = node;
            }
        }

        return bestNode;
    }

    public bool HasAvailableResources()
    {
        foreach (var node in activeResources)
        {
            if (node != null && node.gameObject.activeInHierarchy && !node.IsOccupied)
                return true;
        }

        return false;
    }

    public void SetResourceSpawnRate(int value)
    {
        spawnIntervals = value;
    }
}