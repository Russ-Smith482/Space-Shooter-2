using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour

 
{
    private Enemy _enemy;

// Start is called before the first frame update
void Start()
    {
        _enemy = transform.parent.GetComponent<Enemy>();

        if (_enemy == null)
        {
            Debug.LogError("Enemy is NULL");
                
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            _enemy.PowerupDetected();
        }
    }
}
