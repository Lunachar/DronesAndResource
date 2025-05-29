using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    public enum Faction { Red, Blue }
    
    public Faction faction;
    public Material material;
    public int resources;

    private void Start()
    {
        ApplyMaterial();
    }

    public void ApplyMaterial()
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && material != null)
        {
            renderer.material = new Material(material);
        } 
    }

    public Material GetMaterial()
    {
        return material;
    }

    public int GetResources()
    {
        return resources;
    }

    public void IncreaseResources()
    {
        resources += 1;
    }
}
