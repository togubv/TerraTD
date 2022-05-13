using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellDestructible : Shell
{
    private bool isDestroyed;

    private void Start()
    {
        Destroy(gameObject, 10.0f);
        anim = GetComponent<Animator>();
    }

    public void SetStats(float newSpeed, float newDamage)
    {
        speed = newSpeed;
        damage = newDamage;
    }

    protected void FixedUpdate()
    {
        transform.Translate(Vector2.right * Time.fixedDeltaTime * speed);
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Mob") && !isDestroyed)
        {
            isDestroyed = true;
            DamageTarget(collider);
        }
    }

    protected void DamageTarget(Collider2D collider)
    {
        Damage(collider.gameObject);
        StartCoroutine(DelayedDestroy());
    }

    protected void Damage(GameObject go)
    {
        go.GetComponent<IDamageable>().ApplyDamage(damage);
    }

    private IEnumerator DelayedDestroy()
    {
        //anim.SetTrigger("Destroying");
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
