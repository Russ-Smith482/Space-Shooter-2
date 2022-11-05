using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;
    

    [SerializeField]
    private GameObject[] _commonEnemy;
    [SerializeField]
    private GameObject[] _rareEnemy;
    [SerializeField]
    private GameObject[] _ultraEnemy;

    [SerializeField]
    private GameObject _boss;

    [SerializeField] 
    private GameObject[] _commonPowerups;
    [SerializeField]
    private GameObject[] _rarePowerups;
    [SerializeField]
    private GameObject[] _ultraPowerups;

    [SerializeField]
    private GameObject[] _commonCollectable;
    [SerializeField]
    private GameObject[] _rareCollectable;
    [SerializeField]
    private GameObject[] _ultraCollectable;

    [SerializeField]
    private int _waveNumber;
    private int _enemiesDestroyed;
    private int _maxEnemies;
    private int _enemiesLeftToSpawn;

    private bool _stopSpawningEnemy = false;

    private UIManager _uiManager;
    
    // Start is called before the first frame update
    void Start()
    {

        _uiManager = GameObject.FindObjectOfType<UIManager>();

    }

    public void StartSpawning(int waveNumber)
    {
        

        if (waveNumber <= 5)
        {
            _stopSpawningEnemy = false;
            _enemiesDestroyed = 0;
            _waveNumber = waveNumber;
            _uiManager.UpdateWaves(_waveNumber);
            _enemiesLeftToSpawn = _waveNumber + 6;
            _maxEnemies = _waveNumber + 6;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnCollectableRoutine());
            
        }
         else if (waveNumber == 6)
        {
            Vector3 posToSpawn = new Vector3(-5.5f, 5.6f, 0);
            
            _stopSpawningEnemy = true;
            Instantiate(_boss, posToSpawn, Quaternion.identity);
            _uiManager.BossLifeBar();
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawningEnemy == false && _enemiesDestroyed <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            float randomSpawnTime = Random.Range(3f, 9f);
            int randomEnemyChance = Random.Range(0, 11);
            int commonEnemy = Random.Range(0, _commonEnemy.Length);
            int rareEnemy = Random.Range(0, _rareEnemy.Length);
            int UltraEnemy = Random.Range(0, _ultraEnemy.Length);

            switch (randomEnemyChance)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    Instantiate(_commonEnemy[commonEnemy], posToSpawn, this.transform.rotation);
                    transform.parent = _enemyContainer.transform;
                    break;
                case 7:
                case 8:
                case 9:
                    Instantiate(_rareEnemy[rareEnemy], posToSpawn, this.transform.rotation);
                    transform.parent = _enemyContainer.transform;
                    break;
                case 10:
                    Instantiate(_ultraEnemy[UltraEnemy], posToSpawn, this.transform.rotation);
                    transform.parent = _enemyContainer.transform;
                    break;

            }

           

            _enemiesLeftToSpawn--;
            if (_enemiesLeftToSpawn == 0)
            {
                _stopSpawningEnemy = true;
            }
            yield return new WaitForSeconds(2.0f);
        }
        StartSpawning(_waveNumber + 1);
        yield return new WaitForSeconds(7.5f);
    }

    IEnumerator SpawnPowerupRoutine()
{
    yield return new WaitForSeconds(8.0f);

        while(true)
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
        float randomSpawnTime = Random.Range(10f, 25f);
        int randomPowerupChance = Random.Range(0, 11);
        int commonPowerup = Random.Range(0, _commonPowerups.Length);
        int rarePowerup = Random.Range(0, _rarePowerups.Length);
        int UltraPowerup = Random.Range(0, _ultraPowerups.Length);

        switch (randomPowerupChance)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                Instantiate(_commonPowerups[commonPowerup], posToSpawn, Quaternion.identity);
                break;
            case 7:
            case 8:
            case 9:
                Instantiate(_rarePowerups[rarePowerup], posToSpawn, Quaternion.identity);
                break;
            case 10:
                Instantiate(_ultraPowerups[UltraPowerup], posToSpawn, Quaternion.identity);
                break;
            }

        yield return new WaitForSeconds(randomSpawnTime);
    }
}

    
    IEnumerator SpawnCollectableRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        while (true)

        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            float randomSpawnTime = Random.Range(12f, 25f);
            int randomCollectableChance = Random.Range(0, 11);
            int commonCollectable = Random.Range(0, _commonCollectable.Length);
            int rareCollectable = Random.Range(0, _rareCollectable.Length);
            int UltraCollectable = Random.Range(0, _ultraCollectable.Length);

            switch (randomCollectableChance)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    Instantiate(_commonCollectable[commonCollectable], posToSpawn, Quaternion.identity);
                    break;
                case 7:
                case 8:
                case 9:
                    Instantiate(_rareCollectable[rareCollectable], posToSpawn, Quaternion.identity);
                    break;
                case 10:
                    Instantiate(_ultraCollectable[UltraCollectable], posToSpawn, Quaternion.identity);
                    break;
            }
            yield return new WaitForSeconds(randomSpawnTime);
        }
    }

    public void EnemyDestroyed()
    {
        _enemiesDestroyed++;
    }

    public void OnPlayerDeath()
    {
        _stopSpawningEnemy = true;

    }
}



