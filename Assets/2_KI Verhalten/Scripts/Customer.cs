using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    public enum CustomerStates
    {
        Spawned,
        WalkingAround,
        Idling,
        OnTheWayToShop,
        WaitingOutside,
        WaitingInLine,
        Served,
        OnTheWayHome
        
    }

    private List<Vector3> spots = new List<Vector3>();
    
    [SerializeField] private Vector3 moveTo;
    private bool customerServed = false;
    private bool hasTarget = false;
    private NavMeshAgent navMeshAgent;
    public CustomerStates customerStates;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        
        spots.Add(new Vector3(0f,1f,7f)); // 0 -> Shop
        spots.Add(new Vector3(-5.5f, 1f, -11.5f)); // 1 -> spot 1
        spots.Add(new Vector3(-38.5f,1f,11.5f)); // 2 -> spot2
        spots.Add(new Vector3(-25f,1f,-4f));
        spots.Add(new Vector3(20f,1f,-10f));
        spots.Add(new Vector3(-67f,1f,-7.5f));
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
            case CustomerStates.OnTheWayToShop:
                GoToShop();
                break;
            case CustomerStates.WaitingOutside:
                WaitOutsideOfTheStore();
                break;
            case CustomerStates.WaitingInLine:
                StartCoroutine(WaitingInLine());
                break;
            case CustomerStates.Served:

                break;
            case CustomerStates.OnTheWayHome:

                break;
            
        }
    }

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
            customerStates = CustomerStates.WaitingOutside;
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
        int positionIndex;
        
        if (hasTarget == false)
        {
            // give the npc a new target to visit
            positionIndex = Random.Range(0, spots.Count-1);
            Debug.Log(positionIndex);
            //if (positionIndex == 0) // small chance of visiting the store
            //{
            //    customerStates = CustomerStates.OnTheWayToShop;
            //}
            
            moveTo = spots[positionIndex];
            hasTarget = true;
        }
        else
        {
            navMeshAgent.destination = moveTo;
            
            yield return new WaitForSeconds(30f);
            if (Vector3.Distance(transform.position, moveTo) < 2)
            {
                //positionIndex = Random.Range(0, spots.Count-1); // change target pos with a small % chance of visiting the store
                hasTarget = false;
            }

            

        }
            
    }

    IEnumerator WaitingInLine()
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

    
