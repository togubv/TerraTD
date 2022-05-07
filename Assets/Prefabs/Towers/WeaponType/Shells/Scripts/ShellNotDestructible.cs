using System.Collections;
using UnityEngine;

public class ShellNotDestructible : Shell
{
    private void Start()
    {
        Destroy(gameObject, 10.0f);
    }

    public void SetStats(float newSpeed, float newDamage)
    {
        speed = newSpeed;
        damage = newDamage;
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

    protected void DamageTarget(Collider2D collider)
    {
        Damage(collider.gameObject);
    }

    protected void Damage(GameObject go)
    {
        go.GetComponent<IDamageable>().ApplyDamage(damage);
    }
}