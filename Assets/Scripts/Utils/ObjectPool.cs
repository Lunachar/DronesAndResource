using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Queue<T> pool;
    private readonly int initialSize;
    
    public ObjectPool(T prefab, Transform parent, int initialSize = 10)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.initialSize = initialSize;
        this.pool = new Queue<T>();
        
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewInstance();
        }
    }
    
    private void CreateNewInstance()
    {
        var instance = Object.Instantiate(prefab, parent);
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }
    
    public T Get()
    {
        if (pool.Count == 0)
        {
            CreateNewInstance();
        }
        
        var instance = pool.Dequeue();
        instance.gameObject.SetActive(true);
        return instance;
    }
    
    public void Return(T instance)
    {
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }
    
    public void Clear()
    {
        while (pool.Count > 0)
        {
            var instance = pool.Dequeue();
            Object.Destroy(instance.gameObject);
        }
    }
} 