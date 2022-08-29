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

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

// Update is called once per frame
void Update()
    {
        //rotate object on z axis 
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            
            _spawnManager.StartSpawning();
            
            Destroy(this.gameObject, 0.25f);
            
        }
    }
}
