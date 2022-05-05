using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Bank bank;
    [SerializeField] private GameObject prefab_unit;
    [SerializeField] private GameObject scene_objects;
    [SerializeField][Range(0, 3)] private float unit_speed;
    [SerializeField] private GameObject[] cellsSpawner;

    public void SpawnEnemies()
    {
        Vector2 position = cellsSpawner[Random.Range(0, cellsSpawner.Length)].transform.position;
        position.y += Random.Range(-0.1f, 0.1f);
        GameObject new_unit = Instantiate(prefab_unit, position, Quaternion.identity, scene_objects.transform);
    }
}
