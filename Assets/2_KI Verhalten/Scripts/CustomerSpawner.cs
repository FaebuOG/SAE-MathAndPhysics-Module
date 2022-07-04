using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    List<GameObject> customers = new List<GameObject>();
    
    [SerializeField] private GameObject customerPrefab;
    private int customerCount;
    public int maxCustomerCount;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCustomer()
    {
        if (customerCount < maxCustomerCount)
        {
            StartCoroutine(SpawnNewCustomer());
        }
    }

    IEnumerator SpawnNewCustomer()
    {
        yield return new WaitForSeconds(5);
        GameObject newCustomer = Instantiate(customerPrefab);

    }
}
