using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public bool IsOccupied { get; set; } = false;

    public void OnGathered()
    {
        DroneManager.Instance.UnregisterResource(this);
        gameObject.SetActive(false);
    }
    
    public void MarkOccupiedTemporarily(float delay = 3f)
    {
        IsOccupied = true;
        CancelInvoke(nameof(ReleaseOccupied)); // На случай повторного вызова
        Invoke(nameof(ReleaseOccupied), delay);
    }

    private void ReleaseOccupied()
    {
        IsOccupied = false;
    }

}
