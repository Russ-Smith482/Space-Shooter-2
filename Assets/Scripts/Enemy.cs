using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int enemyID; //0 = standard, 1 = left, 2 = right

    [SerializeField]
    private float _speed = 4f;
    
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private float _fireRate = 3f;
    private float _canFire = -1;
    [SerializeField]
    private Animator _animator;

    private AudioSource _audioSource;

    private bool _isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();


        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator is NULL.");
        }

    }

    // Update is called once per frame
    void Update()
    {
         EnemyMovement();

        if (Time.time > _canFire && _isDead == false)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, this.transform.rotation);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void EnemyMovement()
    {
        switch(enemyID)
        {
            case 0:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);

                if (transform.position.y < -6f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }
                break;

            case 1:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, 35);

                if (transform.position.y < -6f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }

                else if (transform.position.x >= 11)
                    {
                        transform.position = new Vector3(-11f, transform.position.y, 0);
                    }

                break;

            case 2:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, -35);

                if (transform.position.y < -6f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }

                    else if (transform.position.x <= -11)
                    {
                        transform.position = new Vector3(11f, transform.position.y, 0);
                    }
                
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _isDead = true;
            Destroy(this.gameObject, 2.6f);

        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            _isDead = true;
            Destroy(this.gameObject, 2.6f);
        }

        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            _isDead = true;
            Destroy(this.gameObject, 2.6f);
        }

    }
}
