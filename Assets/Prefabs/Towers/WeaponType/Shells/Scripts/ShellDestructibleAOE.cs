using System.Collections;
using UnityEngine;

public class ShellDestructibleAOE : Shell
{
    private float radius;
    private Collider2D[] hitColliders;

    private void Start()
    {
        radius = 1.0f;
        Destroy(gameObject, 10.0f);
        anim = GetComponent<Animator>();
    }

    public void SetStats(float newSpeed, float newDamage, float newRadius)
    {
        speed = newSpeed;
        damage = newDamage;
        radius = newRadius;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * Time.fixedDeltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Mob"))
        {
            DamageTarget(collider);
        }
    }

    private void DamageTarget(Collider2D collider)
    {

        hitColliders =  Physics2D.OverlapCircleAll(collider.transform.position, radius, 1 << 10);
        foreach (Collider2D col in hitColliders)
        {
                Damage(col.gameObject);
        }
        StartCoroutine(DelayedDestroy());
    }

    private void Damage(GameObject go)
    {
        go.GetComponent<IDamageable>().ApplyDamage(damage);
    }

    private IEnumerator DelayedDestroy()
    {
        anim.SetTrigger("Destroying");
        yield return new WaitForSeconds(0.2f);
        
        Destroy(gameObject);
    }
}