using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _missileSpeed = 5.0f;
    [SerializeField]
    private float _rotatespeed = 350f;

    private GameObject _closeEnemy;
    private Rigidbody2D _rb;
   
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_closeEnemy == null)
        {
            _closeEnemy = FindCloseEnemy();
        }
        if (_closeEnemy != null)
        {
            MoveTowardsEnemy();
        }
        else
        {
            transform.Translate(Vector3.up * _missileSpeed * Time.deltaTime);

            if (transform.position.y >= 8)
            { 
                Destroy(this.gameObject);
            }
        }
    }

    private GameObject FindCloseEnemy()
    {
        try
        {
            GameObject[] enemies;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject close = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;

            foreach (GameObject enemy in enemies)
            {

                Vector3 other = enemy.transform.position - position;
                float curDistance = other.sqrMagnitude;
                if (curDistance < distance)
                {
                    close = enemy;
                    distance = curDistance;
                }
            }
            return close;
        }
        catch

        {
            return null;
        }
    }

    private void MoveTowardsEnemy()
    { 
        Vector2 direction = (Vector2)_closeEnemy.transform.position - _rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
       _rb.angularVelocity = -rotateAmount * _rotatespeed;
        _rb.velocity = transform.up * _missileSpeed;
    }
}
