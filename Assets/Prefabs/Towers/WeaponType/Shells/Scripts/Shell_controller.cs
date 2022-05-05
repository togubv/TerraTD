using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_controller : MonoBehaviour
{
    private float speed, damage;

    private void Start()
    {
        Destroy(gameObject, 10.0f);    
    }

    public void SetStats(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * Time.fixedDeltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Mob"))
        {
            Damage(collider.gameObject);
            Destroy(gameObject);
        }
    }

    private void Damage(GameObject go)
    {
        go.GetComponent<IDamageable>().ApplyDamage(damage);
    }
}
