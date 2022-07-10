using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    public static Customer Instance;
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
    
    //public MovementPoint[] Positions;
    //int totalWeight = 0;
    
    private List<Vector3> spots = new List<Vector3>();
    public CustomerStates customerStates;
    [SerializeField] private Vector3 moveTo;
    [SerializeField] private bool customerServed = false;
    [SerializeField] private bool hasTarget = false;
    private NavMeshAgent navMeshAgent;
    
    // Customer stats
    [SerializeField] [Range(0,100)] private float customerSatisfaction;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        // gets the total weight of all positions
        //for (int i = 0; i < Positions.Length; i++) {
        //    if (Positions[i]!=null) {
        //        totalWeight += Positions[i].Weight;
        //    }
        //}
        
        if (Instance == null) 
            Instance = this;
        
        
        spots.Add(new Vector3(0f,1f,7f)); // 0 -> Shop
        spots.Add(new Vector3(-7.5f, 1f, -11.5f)); // 1 -> spot 1
        spots.Add(new Vector3(-38.5f,1f,11.5f)); // 2 -> spot2
        spots.Add(new Vector3(-28f,1f,-4f));
        spots.Add(new Vector3(23f,1f,-10f));
        spots.Add(new Vector3(-72f,1f,-7.5f));
    }
    private void Update()
    {
        switch (customerStates)
        {
            case CustomerStates.Spawned:
                navMeshAgent.destination = new Vector3(-72f, 1f, 0f);
                break;
            case CustomerStates.WalkingAround:
                StartCoroutine(WalkAround());
                break;
            case CustomerStates.Idling:
                break;
            case CustomerStates.OnTheWayToStore:
                GoToShop();
                break;
            case CustomerStates.WaitingOutsideStore:
                WaitOutsideOfTheStore();
                break;
            case CustomerStates.WaitingInsideStore:
                StartCoroutine(WaitingInsideStore());
                break;
            case CustomerStates.Served:

                break;
            case CustomerStates.OnTheWayHome:

                break;
            
        }
    }
    
    //public static Transform GetRandomTransform() {
    //    return Instance.GetRandomPoint();
    //}
    //
    //public Transform GetRandomPoint() {
    //
    //    int rnd = Random.Range(0, totalWeight);
    //    Transform returnValue = null;
    //
    //    for (int i = 0; i < Positions.Length; i++) {
    //        if (Positions[i] != null) {
    //            rnd -= Positions[i].Weight;
    //        }
    //        if (rnd<=0) {
    //            returnValue = Positions[i].Target;
    //            break;
    //        }
    //    }
    //
    //    return returnValue;
    //}

    public void GoToShop()
    {
        // when store is open and not to much people inside
        if (ShopManager.Instance.StoreIsOpen == true)
        {
            navMeshAgent.destination = new Vector3(0f, 1f, 0f);
        }
        else
        {
            Debug.Log("Sorry, the store is currently closed. You must wait outside");
            customerStates = CustomerStates.WaitingOutsideStore;
        }
    }

    public void WaitOutsideOfTheStore()
    {

    }

    public void GoHome()
    {
        navMeshAgent.destination = new Vector3(-46f, 1f, 5f);
    }

    IEnumerator WalkAround()
    {
        int random;

        if (hasTarget == false)
        {
            // give the npc a new target to visit
            var positionIndex = Random.Range(0, spots.Count-1);
            moveTo = spots[positionIndex];
            hasTarget = true;
        }
        else
        {
            navMeshAgent.destination = moveTo;
            yield return new WaitForSeconds(5f);
            if (Vector3.Distance(transform.position, moveTo) < 2)
            {
                navMeshAgent.destination = Vector3.zero;
                yield return new WaitForSeconds(20f);
                hasTarget = false;
            }
        }
    }

    IEnumerator WaitingInsideStore()
    {
        if (customerStates != CustomerStates.Served)
        {
            yield return new WaitForSeconds(15);
            GoHome();
        }
        else
        {
            Debug.Log("Thank you, see you soon");
            GoHome();
            yield break;
        }
    }
}

    
