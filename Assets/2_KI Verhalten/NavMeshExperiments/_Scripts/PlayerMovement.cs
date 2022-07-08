using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] [RequireComponent(typeof(AgentLinkMover))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera Camera = null;
    [SerializeField] private Animator animator;
    private NavMeshAgent navMeshAgent;
    private AgentLinkMover agentLinkMover;
    private RaycastHit[] Hits = new RaycastHit[1];
    
    private const string isWalking = "isWalking";
    private const string jump = "jump";
    private const string landed = "landed";
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        agentLinkMover = GetComponent<AgentLinkMover>();

        agentLinkMover.OnLinkStart += HandleLinkStart;
        agentLinkMover.OnLinkEnd += HandleLinkEnd;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.RaycastNonAlloc(ray, Hits) > 0)
            {
                navMeshAgent.SetDestination(Hits[0].point);
            }
        }
        
        animator.SetBool(isWalking, navMeshAgent.velocity.magnitude > 0.01f);
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
