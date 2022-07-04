using UnityEngine;

public class ShopArea : MonoBehaviour
{
   private Customer customer;
   private void OnTriggerEnter(Collider other)
   {
      //if(other.CompareTag("Customer"))
         Debug.Log("Worked");
   }
}
