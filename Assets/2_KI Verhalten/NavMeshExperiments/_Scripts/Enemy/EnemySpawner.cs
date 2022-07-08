using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
   public Transform Player;
   public int NumberOfEnemiesToSpawn = 5;
   public float SpawnDelay = 1f;
   
   public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;
   
   public List<Enemy> EnemyPrefabs = new List<Enemy>();
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

   private void SpawnRoundRobinEnemy(int spawnedEnemies)
   {
      int spawnIndex = spawnedEnemies % EnemyPrefabs.Count;

      DoSpawnEnemy(spawnIndex);
   }
   private void SpawnRandomEnemy()
   {
      DoSpawnEnemy(UnityEngine.Random.Range(0, EnemyPrefabs.Count));
      
   }
   
   private void DoSpawnEnemy(int spawnIndex)
   {
      PoolableObject poolableObject = EnemyObjectPools[spawnIndex].GetObject();

      if (poolableObject != null)
      {
         Enemy enemy = poolableObject.GetComponent<Enemy>();

         NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

         int vertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);

         NavMeshHit hit;
         if(NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, -1))
         {
            enemy.Agent.Warp(hit.position);
            // enemy need to get enabled and start chasing now
            enemy.Movement.Player = Player;
            enemy.Agent.enabled = true;
            enemy.Movement.StartChasing();
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
   public enum SpawnMethod
   {
      RoundRobin,
      Random
   }
}
