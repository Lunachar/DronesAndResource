using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    private bool isClaimed = false;
    public bool IsClaimed => isClaimed;
    
    public void Claim() => isClaimed = true;
    
}
