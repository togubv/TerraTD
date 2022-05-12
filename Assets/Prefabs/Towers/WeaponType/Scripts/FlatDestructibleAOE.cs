using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatDestructibleAOE : TowerWeapon
{
    public List<GameObject> enemies_list;
    public bool attacking;
    private bool isCooldown;

    private float radius;

    private void Awake()
    {
        TowerCard card = GetComponentInParent<TowerConfuguration>().Card;
        this.shell = card.shell;
        this.cooldown = card.cooldown;
        this.speed = card.shell_speed;
        this.damage = card.damage;
        this.radius = card.shell_radius;
    }

    private void FixedUpdate()
    {
        if (attacking && !isCooldown)
        {
            GameObject new_shell = Instantiate(shell, transform);
            new_shell.GetComponent<ShellDestructibleAOE>().SetStats(speed, damage, radius);
            isCooldown = true;
            StartCoroutine(StartCooldownTimer());
        }
    }

    private IEnumerator StartCooldownTimer()
    {
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Mob"))
        {
            enemies_list.Add(collider.gameObject);
            attacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Mob"))
        {
            enemies_list.Remove(collider.gameObject);
            if (enemies_list.Count < 1)
            {
                attacking = false;
            }
        }
    }
}