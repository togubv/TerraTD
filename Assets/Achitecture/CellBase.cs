using System.Collections;
using UnityEngine;

public class CellBase : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Mob"))
        {
            levelManager.DecreasePlayerHP(this, 1);
            Destroy(collider.gameObject);
        }
    }
}