using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject[] collectables;
    [SerializeField]
    private GameObject _missilePowerup;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
   

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnCollectableRoutine());
        StartCoroutine(SpawnMissilePowerup());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomEnemy = Random.Range(0, 3);
            GameObject newEnemy = Instantiate(enemies[randomEnemy], posToSpawn, this.transform.rotation);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 4);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }

    }

    IEnumerator SpawnCollectableRoutine()
    {
        yield return new WaitForSeconds(8.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomCollectable = Random.Range(0, 2);
            Instantiate(collectables[randomCollectable], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(9f, 18f));
        }
    }

    IEnumerator SpawnMissilePowerup()
    {
        yield return new WaitForSeconds(25.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(_missilePowerup, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(20.0f, 40.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
