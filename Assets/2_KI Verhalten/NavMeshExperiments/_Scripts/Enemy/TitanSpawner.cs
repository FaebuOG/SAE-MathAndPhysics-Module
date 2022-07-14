using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class TitanSpawner : MonoBehaviour
{
   [Header("Settings")]
   public Transform Player;
   public int NumberOfEnemiesToSpawn;
   public float SpawnDelay;
   public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;
   
   [Header("Object Pool")]
   public List<Titan> EnemyPrefabs = new List<Titan>();
   private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();
   
   private NavMeshTriangulation triangulation;
   
   private void Awake()
   {
      for (int i = 0; i < EnemyPrefabs.Count; i++)
      {
         EnemyObjectPools.Add(i, ObjectPool.CreateInstance(EnemyPrefabs[i], NumberOfEnemiesToSpawn));
      }
   }
   private void Start()
   {
      triangulation = NavMesh.CalculateTriangulation(); // expensive method
      StartCoroutine(SpawnEnemies());
   }

   

   #region Spawn algorithm
   public enum SpawnMethod
   {
      RoundRobin,
      Random
   }
   private void SpawnRoundRobinEnemy(int spawnedEnemies)
   {
      int spawnIndex = spawnedEnemies % EnemyPrefabs.Count;
      DoSpawnEnemy(spawnIndex);
   }
   private void SpawnRandomEnemy()
   {
      DoSpawnEnemy(UnityEngine.Random.Range(0, EnemyPrefabs.Count));
   }
   #endregion
   
   private IEnumerator SpawnEnemies()
   {
      WaitForSeconds wait = new WaitForSeconds(SpawnDelay);

      int spawnedEnemies = 0;
      while (spawnedEnemies < NumberOfEnemiesToSpawn)
      {
         if (EnemySpawnMethod == SpawnMethod.RoundRobin)
         {
            SpawnRoundRobinEnemy(spawnedEnemies);
         }
         else if (EnemySpawnMethod == SpawnMethod.Random)
         {
            SpawnRandomEnemy();
         }

         spawnedEnemies++;
         yield return wait;
      }
   }
   private void DoSpawnEnemy(int spawnIndex)
   {
      PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();

      if (poolableObject != null)
      {
         Titan titan = poolableObject.GetComponent<Titan>();

         NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

         int vertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);

         NavMeshHit hit;
         if(NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, -1))
         {
            titan.Agent.Warp(hit.position);
            
            // titan need to get enabled and start chasing now
            titan.Movement.Player = Player;
            titan.Agent.enabled = true;
            titan.Movement.StartChasing();
         }
         else
         {
            Debug.LogError("Unable to place NavmeshAgent on Navmesh. Tried to use " +  triangulation.vertices[vertexIndex]);
         }
      }
      else
      {
         Debug.LogError("Unable to fetch enemy of type " + spawnIndex + "from Object pool. Out of Objects?");
      }
   }
}
