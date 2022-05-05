using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobConfiguration : Unit
{
    [SerializeField] private MobCard card;
    private Animator anim;

    private void Awake()
    {
        maxHP = card.max_hp;
        CurrentHP = maxHP;

        anim = GetComponent<Animator>();
        GetComponent<MobController>().SetStats(card);
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
        DamagedAnimation();
    }

    public override void Die()
    {
        Debug.Log("MOB DIED!");
        Destroy(gameObject);
    }
}
