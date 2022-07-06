using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    List<GameObject> customers = new List<GameObject>();
    
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private int populationCount = 0;
    [SerializeField] private int maxPopulationCount;
    private bool gameIsRunning = true;
    
    private void Awake()
    {
        StartCoroutine(SpawnNewCustomer());
    }

    IEnumerator SpawnNewCustomer()
    {
        while (gameIsRunning)
        {
            if (!gameIsRunning)
                break;
            
            if (populationCount < maxPopulationCount)
            {
                yield return new WaitForSeconds(5);
                Instantiate(customerPrefab, new Vector3(-72f, 1f, 20f), Quaternion.identity);
                populationCount += 1;
            }
            else
            {
                yield return new WaitForSeconds(5);
            }
        }
        
    }
}
