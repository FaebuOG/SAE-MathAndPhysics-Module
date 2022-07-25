using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Titan : PoolableObject, IDamageable
{
    public AttackRadius AttackRadius;
    public Animator Animator;
    public TitanMovement Movement;
    public NavMeshAgent Agent;
    private TitanScriptableObject titanScriptableObject;
    public int Health;

    private Coroutine lookCoroutine;
    private const string attackTrigger = "attack";

    private void Awake()
    {
        // Events
        AttackRadius.OnAttack += OnAttack;
    }

    #region Orientation
    private IEnumerator LookAt(Transform target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }
    
    public Transform GetTransform()
    {
        return transform;
    }
    #endregion
    
    #region Enable/Disable
    public virtual void OnEnable()
    {
        SetupAgentFromConfiguration();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }
    #endregion
    
    #region Attack
    private void OnAttack(IDamageable target)
    {
        Animator.SetTrigger(attackTrigger);

        if (lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt(target.GetTransform()));
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
    
    // Gets all the values from the Titan Scriptable Object
    public virtual void SetupAgentFromConfiguration()
    {
        Agent.acceleration = titanScriptableObject.Acceleration;
        Agent.angularSpeed = titanScriptableObject.AngularSpeed;
        Agent.areaMask = titanScriptableObject.AreaMask;
        Agent.avoidancePriority = titanScriptableObject.AvoidancePriority;
        Agent.baseOffset = titanScriptableObject.BaseOffset;
        Agent.height = titanScriptableObject.Height;
        Agent.obstacleAvoidanceType = titanScriptableObject.ObstacleAvoidanceType;
        Agent.radius = titanScriptableObject.Radius;
        Agent.speed = titanScriptableObject.Speed;
        Agent.stoppingDistance = titanScriptableObject.StoppingDistance;
        
        Movement.UpdateRate = titanScriptableObject.AIUpdateInterval;

        Health = titanScriptableObject.Health;

        (AttackRadius.Collider == null ? AttackRadius.GetComponent<SphereCollider>() : AttackRadius.Collider).radius = titanScriptableObject.AttackRadius;
        AttackRadius.AttackDelay = titanScriptableObject.AttackDelay;
        AttackRadius.Damage = titanScriptableObject.Damage;
    }
}