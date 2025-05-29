// using System;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.UI;
//
// public class DroneAI : MonoBehaviour
// {
//     [SerializeField] private GameObject progressCanvas;
//     [SerializeField] private Slider progressBar;
//
//     private ResourceManager resourceManager;
//     private Transform homeBase;
//
//     private NavMeshAgent agent;
//
//     private enum State
//     {
//         Idle,
//         MovingToResource,
//         Collecting,
//         Returning
//     }
//
//     public enum Faction
//     {
//         Blue,
//         Red
//     }
//     
//     public ResourceNode CurrentTarget { get; set; }
//
//     private State currentState = State.Idle;
//     private Faction faction;
//
//     private ResourceNode targetResource;
//     private DroneController controller;
//
//     private bool isCollecting = false;
//
//     
//     private void Start()
//     {
//         controller = GetComponent<DroneController>();
//
//         GameObject managerObject = GameObject.FindWithTag("ResourceManager");
//         if (managerObject != null)
//             resourceManager = managerObject.GetComponent<ResourceManager>();
//
//         agent = GetComponent<NavMeshAgent>();
//         StartCoroutine(StateMachine());
//     }
//
//     IEnumerator StateMachine()
//     {
//         while (true)
//         {
//             switch (currentState)
//             {
//                 case State.Idle:
//                     FindResource();
//                     break;
//
//                 case State.MovingToResource:
//                     if (targetResource == null) SetState(State.Idle);
//                     else if (!agent.pathPending && agent.remainingDistance < 0.5f)
//                         SetState(State.Collecting);
//                     break;
//
//                 case State.Collecting:
//                     if (!isCollecting)
//                     {
//                         isCollecting = true;
//                         StartCoroutine(CollectRoutine());
//                     }
//
//                     break;
//
//                 case State.Returning:
//                     if (!agent.pathPending && agent.remainingDistance < 0.5f)
//                     {
//                         // effect
//                         SetState(State.Idle);
//                     }
//
//                     break;
//             }
//
//             yield return null;
//         }
//     }
//
//     private IEnumerator CollectRoutine()
//     {
//         progressCanvas.SetActive(true);
//         progressBar.value = 0;
//
//         float collectTime = 2f;
//         float elapsed = 0f;
//
//         while (elapsed < collectTime)
//         {
//             elapsed += Time.deltaTime;
//             progressBar.value = Mathf.Clamp01(elapsed / collectTime);
//             yield return null;
//         }
//
//         if (targetResource != null) Destroy(targetResource.gameObject);
//         SetState(State.Returning);
//         isCollecting = false;
//
//         progressCanvas.SetActive(false);
//         progressBar.value = 0;
//     }
//
//     private void SetState(State newState)
//     {
//         currentState = newState;
//
//         switch (newState)
//         {
//             case State.MovingToResource:
//                 if (targetResource != null)
//                     agent.SetDestination(targetResource.transform.position);
//                 break;
//
//             case State.Returning:
//                 agent.SetDestination(homeBase.position);
//                 break;
//         }
//     }
//
//     public void SetFaction(Faction newFaction)
//     {
//         faction = newFaction;
//
//         if (faction == Faction.Blue)
//             homeBase = GameObject.FindGameObjectWithTag("BlueBase").transform;
//         else
//             homeBase = GameObject.FindGameObjectWithTag("RedBase").transform;
//     }
//
//     public Faction GetFaction()
//     {
//         return faction;
//     }
//
//     private void FindResource()
//     {
//         if (resourceManager == null || homeBase == null) return;
//
//         var res = resourceManager.GetBestResource(transform.position, homeBase.position);
//         if (res != null)
//         {
//             res.Claim();
//             targetResource = res;
//             SetState(State.MovingToResource);
//         }
//     }
//
//     public void SetSpeed(int value)
//     {
//         if (agent != null)
//             agent.speed = value;
//     }
//
//     public void TogglePathRendering(bool show)
//     {
//         var line = GetComponent<LineRenderer>();
//         if (line != null)
//             line.enabled = show;
//     }
//
//     public void ResetDrone()
//     {
//         isCollecting = false;
//         StopAllCoroutines();
//         currentState = State.Idle;
//         progressCanvas.SetActive(false);
//         progressBar.value = 0;
//         StartCoroutine(StateMachine());
//     }
//
//     public void AnimateSpawn()
//     {
//         transform.localScale = Vector3.zero;
//         gameObject.SetActive(true);
//         StartCoroutine(ScaleUp());
//     }
//
//     private IEnumerator ScaleUp()
//     {
//         Vector3 targetScale = Vector3.one;
//         float t = 0f;
//         while (t < 1)
//         {
//             t += Time.deltaTime * 2f;
//             transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
//             yield return null;
//         }
//     }
//
//     public void SetTarget(ResourceNode node)
//     {
//         CurrentTarget = node;
//         CurrentTarget.Claim();
//     }
//     
//
// }