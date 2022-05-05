using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float CurrentHP { get; set; }
    void ApplyDamage(float damage);
}

public abstract class Unit : MonoBehaviour, IDamageable
{
    protected float maxHP; //damage, cooldown;
    public float CurrentHP { get;  set; }
    public abstract void ApplyDamage(float damage);
    public abstract void Die();
    public abstract void DamagedAnimation();
}
