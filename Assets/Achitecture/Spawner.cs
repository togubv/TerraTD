using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Bank bank;
    [SerializeField] private GameObject[] prefabUnit;
    [SerializeField] private GameObject sceneObjects;
    [SerializeField] private GameObject[] cellsSpawner;

    public List<GameObject> mobPool;

    public int waveMobCountMax = 3;
    public int waveMobCountCurrent;
    public float minDelay = 4;
    public float maxDelay = 8;

    private bool spawnElements;

    private void Start()
    {
        waveMobCountMax = 0;
        StartNewWave();
    }

    private void StartNewWave()
    {
        Debug.Log("START NEW WAVE");
        waveMobCountMax += 1;
        waveMobCountCurrent = 0;
        if (minDelay >= 2) minDelay -= 1;
        if (maxDelay >= 1.5f) maxDelay -= 1.5f;
        if (spawnElements) SpawnEnemies(Random.Range(0, 4));
        StartCoroutine(DelaySpawnMob(this.minDelay, this.maxDelay));
    }

    private IEnumerator DelaySpawnMob(float minDelay, float maxDelay)
    {
        spawnElements = true;
        while (waveMobCountCurrent < waveMobCountMax)
        {
            float randomDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomDelay);
            SpawnEnemies(4);
        }
        StartNewWave();
    }

    public void SpawnEnemies(int unitID)
    {
        Vector2 position = cellsSpawner[Random.Range(0, cellsSpawner.Length)].transform.position;
        position.y += Random.Range(-0.1f, 0.1f);
        GameObject newUnit = Instantiate(prefabUnit[unitID], position, Quaternion.identity, sceneObjects.transform);
        newUnit.GetComponent<MobConfiguration>().SetSpawner(this);
        newUnit.GetComponent<MobConfiguration>().SetBank(bank);
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
