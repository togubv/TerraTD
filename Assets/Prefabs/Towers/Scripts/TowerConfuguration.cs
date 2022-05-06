using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerConfuguration : Unit
{
    [SerializeField] private TowerCard card;

    private TowerBuilder tower_builder;
    private SpriteRenderer spriteR;
    private Animator anim;
    private int number;

    public TowerCard Card => card;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteR = GetComponent<SpriteRenderer>();
        spriteR.sprite = card.sprite;

        maxHP = card.max_hp;
        CurrentHP = maxHP;

        if (card.weapon_type != null) Instantiate(card.weapon_type, gameObject.transform);
        tower_builder = GetComponentInParent<TowerBuilder>();
    }

    public void SetNumber(int i)
    {
        this.number = i;
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
        Debug.Log("TOWER DIED!");
        tower_builder.RemoveTowerFromGrid(number);
        //if (card.income > 0) bank
        Destroy(gameObject);
    }
}
