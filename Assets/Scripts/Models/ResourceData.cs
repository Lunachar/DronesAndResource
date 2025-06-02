using UnityEngine;

public class ResourceData
{
    public Vector3 Position { get; set; }
    public bool IsBeingCollected { get; set; }
    public float CollectionStartTime { get; set; }
    
    public ResourceData(Vector3 position)
    {
        Position = position;
        IsBeingCollected = false;
        CollectionStartTime = 0f;
    }
} 