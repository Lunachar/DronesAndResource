using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private DroneManager droneManager;
    public static GameManager Instance;
    
    public bool showPaths = true;

    public UIManager ui;

    private int blueScore = 0;
    private int redScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        
    }

    public void SetResourceSpawnRate(int value)
    {
        resourceManager.SetResourceSpawnRate(value);
    }

    public ResourceManager GetResourceManager()
    {
        return resourceManager;
    }

    public void SetPathRendering(bool value)
    {
        showPaths = value;
        foreach (var drone in DroneManager.Instance.dronePool)
        {
            //if (drone.gameObject.activeInHierarchy)
                drone.TogglePathRendering(value);
        }
    }

    public void AddResourceCollected(Base.Faction faction)
    {
        if(faction == Base.Faction.Blue)
            blueScore++;
        else
            redScore++;
        
        ui.UpdateCounter(blueScore, redScore);
    }
}
