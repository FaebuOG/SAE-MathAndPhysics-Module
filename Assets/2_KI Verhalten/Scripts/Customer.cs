using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    public enum CustomerStates
    {
        WalkingAround,
        Idling,
        OnTheWayToShop,
        WaitingOutside,
        WaitingInLine,
        Served,
        OnTheWayHome
        
    }

    [SerializeField] private Vector3 moveTo;
    
    private bool customerServed = false;
    private NavMeshAgent navMeshAgent;
    public CustomerStates customerStates;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

    private void Update()
    {
        switch (customerStates)
        {
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

    public void SetAwakeState()
    {
        int random = Random.Range(0, 100);
        if (random <= ShopManager.Instance.ShopPopularityPercent) // % chance the customer knows & will visit the store
        {
            Debug.Log(random);
            GoToShop();
        }
    }

    public void GoToShop()
    {
        // when store is open and not to much people inside
        if (ShopManager.Instance.StoreIsOpen == true)
        {
            navMeshAgent.destination = new Vector3(0f, 1f, 5f);
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
        
        
        while (customerStates == CustomerStates.WalkingAround)
        {
            int random;
            float posX = Random.Range(-40, 40);
            float posZ = Random.Range(0, 30);
            moveTo = new Vector3(posX, this.transform.position.y, posZ);
            
            while(Vector3.Distance(transform.position, moveTo) > 2)
            {
                navMeshAgent.destination = moveTo;
                
                yield return new WaitForSeconds(30);
                
                // checks from time to time if the customer wants to visit the store or go home
                random = Random.Range(0, 100);
                if (random <= ShopManager.Instance.ShopPopularityPercent) // % chance the customer knows & will visit the store
                {
                    GoToShop();
                }
                else
                {
                    random = Random.Range(0, 100);
                    if (random <= 10) // 10% chance the customer will go back home
                    {
                        GoHome();
                    }
                }
                
                
                
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

    
