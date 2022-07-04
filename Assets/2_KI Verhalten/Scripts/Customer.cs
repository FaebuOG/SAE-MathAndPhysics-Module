using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

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
        
        // events
        //GameEvents.current.onShopTriggerEnter += OnShopEntrace;
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

    public void GoToShop()
    {
        customerStates = CustomerStates.OnTheWayToShop;
        
    }
    public void GoHome()
    {
        customerStates = CustomerStates.OnTheWayHome;
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

    //private void OnShopEntrace(int id)
    //{
    //    if(id == this.ID)
    //    customerStates = CustomerStates.WaitingInLine;
    //}
}
