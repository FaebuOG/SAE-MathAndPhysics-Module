using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    //public int ID;
    
    public enum CustomerStates
    {
        OnTheWayToShop,
        OnTheWayHome,
        WaitingInLine,
        WalkingRandom,
        Talking
    }

    private bool customerServed = false;
    
    
    private NavMeshAgent navMeshAgent;
    public CustomerStates customerStates;
    
    // navigation positions
    [SerializeField] private Transform shopPos;
    [SerializeField] private Transform spawnPos;

    
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        
        
    }
    private void Update()
    {
        switch(customerStates)
        {
            case CustomerStates.OnTheWayToShop:
                navMeshAgent.destination = shopPos.position;
                break;
            case CustomerStates.OnTheWayHome:
                navMeshAgent.destination = spawnPos.position;
                break;
            case CustomerStates.WaitingInLine:
                StartCoroutine(WaitingInLine());
                break;
            case CustomerStates.WalkingRandom:
                
                break;
            case CustomerStates.Talking:
                break;
        }
    }

    public void SetAwakeState()
    {
        if (ShopManager.Instance.StoreIsOpen)
        {
            int random = Random.Range(0, 100);
            if (random <= ShopManager.Instance.ShopPopularityPercent) // % chance the customer will visit the store
            {
                GoToShop();
            }
        }
        
        // if the customer doesnt know the shop or wont visit yet 
        WalkAround();
        
    }
    public void GoToShop()
    {
        customerStates = CustomerStates.OnTheWayToShop;
        
    }
    public void GoHome()
    {
        customerStates = CustomerStates.OnTheWayHome;
    }

    public void WalkAround()
    {
        
    }

    IEnumerator WaitingInLine()
    {
        if (!customerServed)
        {
            yield return new WaitForSeconds(15);
            GoHome();
        }
        else
        {
            GoHome();
            yield break;
        }
        
    }
}
