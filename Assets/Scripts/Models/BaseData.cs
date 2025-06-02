using UnityEngine;

public class BaseData
{
    public Faction Faction { get; private set; }
    public Vector3 Position { get; set; }
    public int CollectedResources { get; set; }
    public int DroneCount { get; set; }
    
    public BaseData(Faction faction, Vector3 position)
    {
        Faction = faction;
        Position = position;
        CollectedResources = 0;
        DroneCount = 0;
    }
} 