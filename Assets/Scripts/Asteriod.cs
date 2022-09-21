using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
    [SerializeField]
    private float _speed = 17.5f;

    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

// Update is called once per frame
void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            int waveNumber = 1;
            _uiManager.UpdateWaves(waveNumber);
            _spawnManager.StartSpawning(waveNumber);
            
            Destroy(this.gameObject, 0.25f);
            
        }
    }
}
