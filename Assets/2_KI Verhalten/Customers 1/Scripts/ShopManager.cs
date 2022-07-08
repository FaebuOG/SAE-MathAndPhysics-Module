using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private CustomerController customerController;
    private List<Customer> customers = new List<Customer>();
    
    
    [Range(0, 100)] public int ShopPopularityPercent;
    public bool StoreIsOpen = true;
    
    public int CustomerInsideTheStoreCount;
    public int MaxCustomerInsideTheStore;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // if a customer enters the store
        if(other.CompareTag("customer"))
        {
            customerController = other.GetComponent<CustomerController>();
            if (CustomerInsideTheStoreCount < MaxCustomerInsideTheStore)
            {
                // put him in the line
                customerController.customerStates = CustomerController.CustomerStates.WaitingInsideStore;
            }
            else
            {
                // let him wait outside
            }
           
            
        }
    }

    
    public void OpenShop()
    {
        StoreIsOpen = true;
    }
    public void CloseShop()
    {
        StoreIsOpen = false;
    }
}
