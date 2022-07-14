using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] [RequireComponent(typeof(AgentLinkMover))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera camera = null;
    [SerializeField] private Animator animator;
    private NavMeshAgent navMeshAgent;
    private AgentLinkMover agentLinkMover;
    
    // Animations strings
    private const string isWalking = "isWalking";
    private const string jump = "jump";
    private const string landed = "landed";
    
    // Mouse 
    private RaycastHit[] hits = new RaycastHit[1];
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        agentLinkMover = GetComponent<AgentLinkMover>();

        // animation events
        agentLinkMover.OnLinkStart += HandleLinkStart;
        agentLinkMover.OnLinkEnd += HandleLinkEnd;
    }
    private void Update()
    {
        // Move the player with your mouse
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.RaycastNonAlloc(ray, hits) > 0)
            {
                navMeshAgent.SetDestination(hits[0].point);
            }
        }
        
        // Plays a running/walking animation when the Nav Mesh Agent is moving
        animator.SetBool(isWalking, navMeshAgent.velocity.magnitude > 0.01f);
    }

    #region Animation Events for Nav Mesh Links
    private void HandleLinkStart()
    {
        animator.SetTrigger(jump);
    }
    private void HandleLinkEnd()
    {
        animator.SetTrigger(landed);
    }
    #endregion
}
