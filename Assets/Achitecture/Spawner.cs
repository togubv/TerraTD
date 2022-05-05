using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabUnit;
    [SerializeField] private GameObject sceneObjects;
    [SerializeField] private GameObject[] cellsSpawner;

    public List<GameObject> mobPool;

    public int waveMobCountMax;
    public int waveMobCountCurrent;
    public float minDelay = 5;
    public float maxDelay = 8;

    private void Start()
    {
        waveMobCountMax = 5;
        StartNewWave();
    }

    private void StartNewWave()
    {
        Debug.Log("START NEW WAVE");
        waveMobCountMax += 3;
        waveMobCountCurrent = 0;
        if (minDelay >= 2) minDelay -= 1;
        if (maxDelay >= 3) maxDelay -= 1f;
        StartCoroutine(DelaySpawnMob(this.minDelay, this.maxDelay));
    }

    private IEnumerator DelaySpawnMob(float minDelay, float maxDelay)
    {
        while (waveMobCountCurrent < waveMobCountMax)
        {
            float randomDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomDelay);
            SpawnEnemies();
        }
        StartNewWave();
    }

    public void SpawnEnemies()
    {
        Vector2 position = cellsSpawner[Random.Range(0, cellsSpawner.Length)].transform.position;
        position.y += Random.Range(-0.1f, 0.1f);
        GameObject newUnit = Instantiate(prefabUnit, position, Quaternion.identity, sceneObjects.transform);
        newUnit.GetComponent<MobConfiguration>().SetSpawner(this);
        mobPool.Add(newUnit);
        waveMobCountCurrent++;
    }

    public void RemoveMobFromMobPool(GameObject go)
    {
        if (mobPool.Contains(go))
        {
            mobPool.Remove(go);
        }
    }


}
