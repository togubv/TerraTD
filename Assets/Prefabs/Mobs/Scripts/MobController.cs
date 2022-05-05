using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets;

    private MobBevaviour bevaviour;
    private float speed, cooldown = 1.0f, damage;
    private bool isCooldown;

    public void SetStats(MobCard card)
    {
        this.speed = card.speed;
        this.cooldown = card.cooldown;
        this.damage = card.damage;
    }

    private void FixedUpdate()
    {
        if (bevaviour == MobBevaviour.Moving) transform.Translate(Vector2.left * Time.fixedDeltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Tower"))
        {
            targets.Add(collider.gameObject);
        }
        SetBehaviour();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Tower"))
        {
            targets.Remove(collider.gameObject);
        }
        SetBehaviour();
    }

    private void SetBehaviour()
    {
        if (ExistTarget())
        {
            bevaviour = MobBevaviour.Attacking;
            StartAttacking();
        }
        else
        {
            bevaviour = MobBevaviour.Moving;
        }
    }

    private bool ExistTarget()
    {
        if (targets.Count > 0) return true;
        else return false;
    }

    private void StartAttacking()
    {
        if(!isCooldown) StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        if (ExistTarget())
        {
            targets[0].GetComponent<IDamageable>().ApplyDamage(damage);
        }
        isCooldown = false;
        SetBehaviour();
    }
}

enum MobBevaviour
{
    Moving,
    Attacking
}