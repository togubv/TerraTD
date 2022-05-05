using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobConfiguration : Unit
{
    [SerializeField] private MobCard card;

    private Bank bank;
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

    public void SetBank(Bank bank)
    {
        this.bank = bank;
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
        if (card.rewardElementID != 0)
        {
            bank.IncreaseElementCount(this, card.rewardElementID, 1);
        }
        spawner.RemoveMobFromMobPool(gameObject);
        Debug.Log("MOB DIED!");
        Destroy(gameObject);
    }
}
