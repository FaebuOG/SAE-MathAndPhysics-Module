using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecisionZone : MonoBehaviour
{
    private CustomerController customerController;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("customer"))
        {
            // get the customer script
            customerController = other.GetComponent<CustomerController>();
            if (customerController.customerStates == CustomerController.CustomerStates.Spawned)
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
            customerController.customerStates = CustomerController.CustomerStates.OnTheWayToStore;
        }
        else
        {
            // if the npc doesnt know or wont visit the store he should just walk around.
            customerController.customerStates = CustomerController.CustomerStates.WalkingAround;
        }
    }
}
