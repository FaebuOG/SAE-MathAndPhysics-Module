using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private AttackRadius attackRadius;
    [SerializeField] private Animator animator;
    [SerializeField] private int health = 300;
    
    private Coroutine lookCoroutine;
    private const string attackTrigger = "attack";

    private void Awake()
    {
        attackRadius.OnAttack += OnAttack;
    }
    
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

    #region Attack
    private void OnAttack(IDamageable target)
    {
        animator.SetTrigger(attackTrigger);

        if (lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt(target.GetTransform()));
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
    
    public Transform GetTransform()
    {
        return transform;
    }
}