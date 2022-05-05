using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobConfiguration : Unit
{
    [SerializeField] private MobCard card;

    private Spawner spawner;
    private Animator anim;

    private void Awake()
    {
        maxHP = card.max_hp;
        CurrentHP = maxHP;

        anim = GetComponent<Animator>();
        GetComponent<MobController>().SetStats(card);
    }

    public void SetSpawner(Spawner spawner)
    {
        this.spawner = spawner;
    }

    public override void DamagedAnimation()
    {
        anim.SetTrigger("TakeDamage");
    }

    public override void ApplyDamage(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Die();
        }
        else
        {
            DamagedAnimation();
        }
    }

    public override void Die()
    {
        spawner.RemoveMobFromMobPool(gameObject);
        Debug.Log("MOB DIED!");
        Destroy(gameObject);
    }
}
