using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private Customer customer;
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
            if (CustomerInsideTheStoreCount < MaxCustomerInsideTheStore)
            {
                // get the customer script and put him in the line
                customer = other.GetComponent<Customer>();
                customer.customerStates = Customer.CustomerStates.WaitingInLine;
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
