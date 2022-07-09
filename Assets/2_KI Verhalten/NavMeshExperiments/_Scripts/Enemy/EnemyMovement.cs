using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] [RequireComponent(typeof(AgentLinkMover))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent navMeshAgent;
    private AgentLinkMover agentLinkMover;
    [SerializeField] private Animator animator;
    
    public float UpdateRate = .1f; // how frequently it gets updated

    private const string isWalking = "isWalking";
    private const string jump = "jump";
    private const string landed = "landed";

    private Coroutine followCoroutine;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        agentLinkMover = GetComponent<AgentLinkMover>();

        agentLinkMover.OnLinkStart += HandleLinkStart;
        agentLinkMover.OnLinkEnd += HandleLinkEnd;
    }

    public void StartChasing()
    {
        if (followCoroutine == null)
        {
            followCoroutine = StartCoroutine(FollowTarget());
        }
        else
        {
            Debug.LogWarning("called start chasing on enemy that is already chasing, this is likely a bug");
        }
    }
    private void Update()
    {
        animator.SetBool(isWalking, navMeshAgent.velocity.magnitude > 0.01f);
    }
    
    IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(UpdateRate);
        while (enabled)
        {
            navMeshAgent.SetDestination(Player.transform.position);

            yield return wait;
        }
    }
    
    private void HandleLinkStart()
    {
        animator.SetTrigger(jump);
    }
    private void HandleLinkEnd()
    {
        animator.SetTrigger(landed);
    }
}
