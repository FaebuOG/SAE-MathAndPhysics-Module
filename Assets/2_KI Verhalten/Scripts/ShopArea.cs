using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopArea : MonoBehaviour
{
   private Customer customer;
   private List<Customer> customers = new List<Customer>();
   private void OnTriggerEnter(Collider other)
   {
      if(other.CompareTag("customer"))
      {
         customer = other.GetComponent<Customer>();
         customer.customerStates = Customer.CustomerStates.WaitingInLine;
      }
   }

   public void OpenShop()
   {
      
   }
   public void CloseShop()
   {
      
   }
}
