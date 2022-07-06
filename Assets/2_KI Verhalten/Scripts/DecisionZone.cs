using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecisionZone : MonoBehaviour
{
    private Customer customer;
    
    private void OnTriggerEnter(Collider other)
    {
        // if a customer enters the store
        if(other.CompareTag("customer"))
        {
            // get the customer script
            customer = other.GetComponent<Customer>();
            if (customer.customerStates == Customer.CustomerStates.Spawned)
            {
                VisitShopOrNot();
            }
        }
    }
    
    public void VisitShopOrNot()
    {
        int random = Random.Range(0, 100);
        if (random <= ShopManager.Instance.ShopPopularityPercent) // % chance the customer knows & will visit the store
        {
            customer.customerStates = Customer.CustomerStates.OnTheWayToShop;
        }
        else
        {
            // if the npc doesnt know or wont visit the store he should just walk around.
            customer.customerStates = Customer.CustomerStates.WalkingAround;
        }
    }
}
