using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    private ResourceManager resourceManager;
    private Transform homeBase;

    private NavMeshAgent agent;

    private enum State
    {
        Idle,
        MovingToResource,
        Collecting,
        Returning
    }

    public enum Faction
    {
        Blue,
        Red
    }

    private State currentState = State.Idle;
    private Faction faction;

    private ResourceNode targetResource;
    private DroneController controller;

    private void Start()
    {
        controller = GetComponent<DroneController>();
        
        GameObject managerObject = GameObject.FindWithTag("ResourceManager");
        if (managerObject != null)
            resourceManager = managerObject.GetComponent<ResourceManager>();

        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    FindResource();
                    break;

                case State.MovingToResource:
                    if (targetResource == null) SetState(State.Idle);
                    else if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        SetState(State.Collecting);
                    break;

                case State.Collecting:
                    StartCoroutine(CollectRoutine());
                    break;

                case State.Returning:
                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        // drop resource effect
                        SetState(State.Idle);
                    }

                    break;
            }

            yield return null;
        }
    }

    private IEnumerator CollectRoutine()
    {
        yield return new WaitForSeconds(2f);
        if (targetResource != null) Destroy(targetResource.gameObject);
        SetState(State.Returning);
        StartCoroutine(StateMachine());
    }

    private void SetState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.MovingToResource:
                if (targetResource != null)
                    agent.SetDestination(targetResource.transform.position);
                break;
            
            case State.Returning:
                agent.SetDestination(homeBase.position);
                break;
        }
    }

    public void SetFaction(Faction newFaction)
    {
        faction = newFaction;
        
        if (faction == Faction.Blue)
            homeBase = GameObject.FindGameObjectWithTag("BlueBase").transform;
        else
            homeBase = GameObject.FindGameObjectWithTag("RedBase").transform;
    }

    public Faction GetFaction()
    {
        return faction;
    }

    private void FindResource()
    {
        var res = resourceManager.GetClosestFreeResource(transform.position);
        if (res != null)
        {
            res.Claim();
            targetResource = res;
            SetState(State.MovingToResource);
        }
    }
}