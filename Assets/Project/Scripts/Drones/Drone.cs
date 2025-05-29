using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Drone : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject progressCanvas;
    [SerializeField] private Slider progressBar;

    private NavMeshAgent agent;
    public ResourceNode targetNode;

    public Base.Faction faction;
    
    private Material material;
    
    // Behaviour settings
    private enum DroneState {Idle, MovingToResources, Gathering, ReturningToBase}
    private DroneState state = DroneState.Idle;
    private int carriedAmount = 0;
    private const int gatherAmount = 1;
    
    // Effects
    [SerializeField] private GameObject effectPrefab;
    private int resourceFlyCount = 5;
    private float flyDuration = 0.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = DroneManager.Instance.GetMaterial(faction);
        lineRenderer.enabled = false;
        StartCoroutine(IdleCheckRoutine());
    }


    public void Initialize(Base.Faction assignedFaction, Material baseMaterial)
    {
        faction = assignedFaction;
        material = new Material(baseMaterial);
        SetMaterial();
    }

    private IEnumerator IdleCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if ((state == DroneState.Idle || state == DroneState.Gathering) && targetNode == null)
            {
                Transform baseTransform = DroneManager.Instance.GetBaseTransform(faction);
                var node = DroneManager.Instance.GetNearestAvailaleResource(transform.position, baseTransform.position);

                if (node != null)
                {
                    targetNode = node;
                    targetNode.MarkOccupiedTemporarily();
                    MoveTo(targetNode.transform.position);
                    state = DroneState.MovingToResources;
                }
            }
        }
    }
    public void StartWorking()
    {
        if (state != DroneState.Idle) return;
        
        Transform baseTransform = DroneManager.Instance.GetBaseTransform(faction);
        targetNode = DroneManager.Instance.GetNearestAvailaleResource(transform.position, baseTransform.position);
        
        if (targetNode != null)
        {
            targetNode.MarkOccupiedTemporarily();
            MoveTo(targetNode.transform.position);
            state = DroneState.MovingToResources;
        }
    }

    private void MoveTo(Vector3 position)
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(position);
        }
    }

    private void Update()
    {
        if (!gameObject.activeSelf || targetNode == null) return;

        if(lineRenderer.enabled)
            UpdatePathLine();
        
        switch (state)
        {
            case DroneState.MovingToResources:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    StartCoroutine(GatherResource());
                break;
            case DroneState.ReturningToBase:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    DeliverToBase();
                break;
        }
    }

    private void DeliverToBase()
    {
        DroneManager.Instance.GetBase(faction).IncreaseResources();
        GameManager.Instance.AddResourceCollected(faction);


        carriedAmount = 0;
        state = DroneState.Idle;
        StartWorking();
    }

    private IEnumerator GatherResource()
    {
        state = DroneState.Gathering;
        progressCanvas.SetActive(true);
        progressBar.value = 0f;

        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            progressBar.value = elapsed / duration;
            elapsed += Time.deltaTime;
            yield return null;
        }

        progressBar.value = 1f;
        progressCanvas.SetActive(false);
        
        carriedAmount = gatherAmount;

        if (targetNode != null)
        {
            targetNode.IsOccupied = false;
            targetNode.OnGathered();
        }
        MoveToBase();
    }

    private void MoveToBase()
    {
       Transform baseTransform = DroneManager.Instance.GetBaseTransform(faction);
       if (baseTransform != null)
       {
           MoveTo(baseTransform.position);
           state = DroneState.ReturningToBase;
       }
    }

    public void ResetDrone()
    {
        StopAllCoroutines();
        
        state = DroneState.Idle;
        carriedAmount = 0;
        targetNode = null;

        progressBar.value = 0f;
        progressCanvas.SetActive(false);
        
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
        TogglePathRendering(false);

        if (targetNode != null)
        {
            targetNode.IsOccupied = false;
            targetNode = null;
        }
    }
    public void SetSpeed(int value)
    {
        if (agent != null)
             agent.speed = value;
    }

    public void TogglePathRendering(bool show)
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = show;
        }
    }
    private void UpdatePathLine()
    {
        if (!lineRenderer.enabled || agent == null || !agent.hasPath)
            return;

        var path = agent.path;
        if (path.corners.Length < 2) return;

        lineRenderer.positionCount = path.corners.Length;
        lineRenderer.SetPositions(path.corners);
    }



    public void SetMaterial()
    {
        if (material == null)
        {
            Debug.LogWarning("Material is not assigned");
            return;
        }

        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            rend.material = material;
        }
    }
    
    // Effects
    
    public void AnimateSpawn()
    {
        if(gameObject.activeSelf || state != DroneState.Idle)
            return;
        
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        StartCoroutine(ScaleUp());
    }
    
    private IEnumerator ScaleUp()
    {
        Vector3 targetScale = Vector3.one;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * 2f;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }
    }
}