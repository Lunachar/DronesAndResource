using UnityEngine;

public class DroneController : BaseController
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float detectionRadius = 10f;
    
    private DroneView droneView;
    private BaseData homeBase;
    private ResourceData currentTarget;
    private bool hasResource;
    
    private void Awake()
    {
        droneView = GetComponent<DroneView>();
    }
    
    public void Initialize(BaseData homeBase)
    {
        this.homeBase = homeBase;
        hasResource = false;
    }
    
    private void Update()
    {
        if (droneView.IsCollecting)
        {
            if (droneView.HasCollectedLongEnough())
            {
                hasResource = true;
                droneView.FinishCollecting();
            }
            return;
        }
        
        if (hasResource)
        {
            MoveTowardsBase();
        }
        else
        {
            MoveTowardsTarget();
        }
    }
    
    private void MoveTowardsTarget()
    {
        if (currentTarget == null) return;
        
        Vector3 direction = currentTarget.Position - transform.position;
        if (direction.magnitude < 0.1f)
        {
            droneView.StartCollecting();
            return;
        }
        
        MoveAndRotate(direction);
    }
    
    private void MoveTowardsBase()
    {
        Vector3 direction = homeBase.Position - transform.position;
        if (direction.magnitude < 0.1f)
        {
            DeliverResource();
            return;
        }
        
        MoveAndRotate(direction);
    }
    
    private void MoveAndRotate(Vector3 direction)
    {
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    
    private void DeliverResource()
    {
        hasResource = false;
        homeBase.CollectedResources++;
        GameEvents.TriggerResourceDelivered(homeBase);
    }
    
    public void SetTarget(ResourceData target)
    {
        if (!droneView.IsCollecting && !hasResource)
        {
            currentTarget = target;
            droneView.SetTarget(target);
        }
    }
    
    public bool CanCollectResource(ResourceData resource)
    {
        return !droneView.IsCollecting && !hasResource && 
               Vector3.Distance(transform.position, resource.Position) <= detectionRadius;
    }
} 