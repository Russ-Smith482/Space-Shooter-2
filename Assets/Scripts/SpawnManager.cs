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


    private int _waveNumber;
    private int _enemiesDestroyed;
    private int _maxEnemies;
    private int _enemiesLeftToSpawn;

    private bool _stopSpawning = false;
    // Start is called before the first frame update

    private UIManager _uiManager;

    void Start()
    {
        _uiManager = GameObject.FindObjectOfType<UIManager>();
    }

    public void StartSpawning(int waveNumber)
    {

        if (waveNumber <= 5)
        {
            _stopSpawning = false;
            _enemiesDestroyed = 0;
            _waveNumber = waveNumber;
            _uiManager.UpdateWaves(_waveNumber);
            _enemiesLeftToSpawn = _waveNumber + 6;
            _maxEnemies = _waveNumber + 6;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnCollectableRoutine());
            StartCoroutine(SpawnMissilePowerup());
        }
    }

        IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false && _enemiesDestroyed <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomEnemy = Random.Range(0, 3);
            GameObject newEnemy = Instantiate(enemies[randomEnemy], posToSpawn, this.transform.rotation);
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemiesLeftToSpawn--;
            if(_enemiesLeftToSpawn == 0)
            {
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(2.0f);
        }
        StartSpawning(_waveNumber + 1);
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 5);
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

    public void EnemyDestroyed()
    {
        _enemiesDestroyed++;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
