using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    public float initHealth; // 초기 채력
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        health = initHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        health -= damage;
        if(health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void RestoreHealth(float value)
    {
        if(dead) return;
        health += value;
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();
        dead = true;
    }
}
