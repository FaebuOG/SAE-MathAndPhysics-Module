using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackRadius : MonoBehaviour
{
    public SphereCollider Collider;
    public int Damage = 10;
    public float AttackDelay = 0.5f;
    public delegate void AttackEvent(IDamageable target);
    public AttackEvent OnAttack;
    
    private Coroutine attackCoroutine;
    private List<IDamageable> damageables = new List<IDamageable>();
    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    #region OnTrigger Events
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageables.Add(damageable);

            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(Attack());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageables.Remove(damageable);
            if (damageables.Count == 0)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }
    #endregion
    
    private IEnumerator Attack()
    {
        // Attack delay
        WaitForSeconds wait = new WaitForSeconds(AttackDelay);
        yield return wait;

        // for the closest target
        IDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;

        // as long as there are Titans in the attack radius
        while (damageables.Count > 0)
        {
            // gets the closest target
            for (int i = 0; i < damageables.Count; i++)
            {
                Transform damageableTransform = damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = damageables[i];
                }
            }

            if (closestDamageable != null)
            {
                OnAttack?.Invoke(closestDamageable);
                closestDamageable.TakeDamage(Damage);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return wait;

            // removes all the possible targets and cleans it for the next iteration
            damageables.RemoveAll(DisabledDamageables);
        }

        attackCoroutine = null;
    }
    private bool DisabledDamageables(IDamageable damageable)
    {
        return damageable != null && !damageable.GetTransform().gameObject.activeSelf;
    }
}
