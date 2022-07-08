using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform target;
    [SerializeField] private float UpdateSpeed = .1f; // how frequently it gets updated

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(UpdateSpeed);
        while (enabled)
        {
            navMeshAgent.SetDestination(target.transform.position);

            yield return wait;
        }
    }
}
