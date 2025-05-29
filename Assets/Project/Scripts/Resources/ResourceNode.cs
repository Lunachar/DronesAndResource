using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public bool IsOccupied { get; set; } = false;

    public void OnGathered()
    {
        DroneManager.Instance.UnregisterResource(this);
        gameObject.SetActive(false);
    }

}
