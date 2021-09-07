using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float initHealth;
    
    public float health;// {get; protected set;}
    public bool death {get; protected set;}
    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        death = false;
        health = initHealth; //초기 값으로 체력 설정
    }

    public virtual void OnDamage(float damage, Vector3 position, Vector3 normal)
    {
        health -= damage;
        if(health <= 0 && !death)
        {
            Die();
        }
    }

    public virtual void RestoreHealth(float value)
    {
        if(death) return;
        health += value;
    }
    // vi editor
    public virtual void Die() 
    {
        if(OnDeath != null) OnDeath();
        death = true;
    }
}
