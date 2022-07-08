using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    public static CustomerController Instance;
    public enum CustomerStates
    {
        Spawned,
        WalkingAround,
        Idling,
        OnTheWayToStore,
        WaitingOutsideStore,
        WaitingInsideStore,
        Served,
        OnTheWayHome
    }
    public CustomerStates customerStates;
    private NavMeshAgent navMeshAgent;
    private ConnectedWaypoint currentWaypoint;
    private ConnectedWaypoint previousWaypoint;
    
    [SerializeField] private bool customerServed = false;
    [SerializeField] private bool customerWaiting;
    [SerializeField] private float totalWaitTime = 3;
    [SerializeField] private float switchProbability = 0.2f;
    
    private bool travelling;
    private bool waiting;
    private float waitTimer;
    private int waypointsVisited;

    private GameObject[] allWaypoints;
    private GameObject[] keyLocations;

    public int UpdateFrequency = 20;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        keyLocations = GameObject.FindGameObjectsWithTag("KeyLocation");
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        if (navMeshAgent == null)
        {
            Debug.LogError("The navmesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            if (currentWaypoint == null)
            {
                
                
                if (allWaypoints.Length > 0)
                {
                    while (currentWaypoint == null)
                    {
                        int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                        if (startingWaypoint != null)
                        {
                            currentWaypoint = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Failed to find any waypoints");
                }
            }
            
            SetDestination();
        }
    }
    private void Update()
    {
        switch (customerStates)
        {
            case CustomerStates.Spawned:
                //navMeshAgent.destination = new Vector3(-72f, 1f, 0f);
                break;
            case CustomerStates.WalkingAround:
                WalkAround();

                break;
            case CustomerStates.Idling:
                break;
            case CustomerStates.OnTheWayToStore:
                GoToShop();
                break;
            case CustomerStates.WaitingOutsideStore:
                //WaitOutsideOfTheStore();
                break;
            case CustomerStates.WaitingInsideStore:
                StartCoroutine(WaitingInsideStore());
                break;
            case CustomerStates.Served:

                break;
            case CustomerStates.OnTheWayHome:
                GoToSpawn();
                break;
            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyLocation"))
        {
            
        }
    }

    #region Customer -> AllMethods
    
        #region -> Travelling
        private void SetDestination()
        {
            if (waypointsVisited > 0)
            {
                ConnectedWaypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
                previousWaypoint = currentWaypoint;
                currentWaypoint = nextWaypoint;
            }

            Vector3 targetVector = currentWaypoint.transform.position;
            navMeshAgent.SetDestination(targetVector);
            travelling = true;
        }
        private void WalkAround()
        {
            if (travelling && navMeshAgent.remainingDistance <= 1f)
            {
                travelling = false;
                waypointsVisited++;

                if (customerWaiting)
                {
                    waiting = true;
                    waitTimer = 0f;
                }
                else
                {
                    SetDestination();
                }
            }

            if (waiting)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= totalWaitTime)
                {
                    waiting = false;
                    SetDestination();
                }
            }
        }
        private void GoToShop()
        {
            Vector3 targetVector = keyLocations[0].transform.position; // Look in the hierarchy for the index
            navMeshAgent.SetDestination(targetVector);
        }
        private void GoToSpawn()
        {
            Vector3 targetVector = keyLocations[UnityEngine.Random.Range(1,2)].transform.position; // Look in the hierarchy for the index
            navMeshAgent.SetDestination(targetVector);
        }
        #endregion
        #region -> Logic
    IEnumerator WaitingInsideStore()
    {
        if (customerStates != CustomerStates.Served)
        {
            // when the customer isn't served yet he waits for an amount of time till he heads out.
            yield return new WaitForSeconds(15);
            GoToSpawn();
        }
        else
        {
            // when the customer is served
            Debug.Log("Thank you, see you soon");
            GoToSpawn();
            yield break;
        }
    }
    #endregion
    
    #endregion
}
