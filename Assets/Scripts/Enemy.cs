using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int enemyID; //0 = standard, 1 = left, 2 = right, 3 = red, 4 = shielded

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _ramSpeed = 6f;
    [SerializeField]
    private float _detectRange = 6f;
    
    
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldEnemy;
    [SerializeField]
    private GameObject _beam;


    private float _fireRate = 3f;
    private float _canFire = -1;
    [SerializeField]

    private Animator _animator;

    private AudioSource _audioSource;

    private bool _isDead = false;

    private SpawnManager _spawnManager;
    private Player _player;

    private bool _isEnemyRed = false;
    private bool _isEnemyShielded = false;
    private bool _isEnemyShieldDown = false;
    private bool _powerupDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
       
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

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

    }

    // Update is called once per frame
    void Update()
    {
         EnemyMovement();

        if (Time.time > _canFire && _isDead == false && _powerupDetected == true)
        {
            LaserFire();
        }

    }

    void EnemyMovement()
    {
         

        switch (enemyID)
        {

            case 0:
                
                transform.Translate(Vector3.down * _speed * Time.deltaTime);

                if (transform.position.y < -6f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }

                LaserFire();
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
                LaserFire();
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
                LaserFire();
                break;

            case 3:

                transform.Translate((Vector3.down + Vector3.left) * _speed * Time.deltaTime);

                if (transform.position.y <= 4f)

                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                    transform.Translate((Vector3.down + Vector3.right) * Time.deltaTime);
                }

                if (transform.position.y <= 2f)

                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                    transform.Translate((Vector3.down + Vector3.left) * Time.deltaTime);
                }

                if (transform.position.y < -6f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }
                else if (transform.position.x <= -11)
                {
                    transform.position = new Vector3(11f, transform.position.y, 0);
                }

                if (_isEnemyRed == false)
                {
                    EnemyRed();
                }

                break;

            case 4:

                transform.Translate(Vector3.down * _speed * Time.deltaTime);

                if (transform.position.y < -6f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }

                LaserFire();

                if (_isEnemyShielded == false)
                {
                    EnemyShielded();
                }
                break;

            case 5:

                transform.Translate(Vector3.down * _speed * Time.deltaTime);

                if (transform.position.y < -6f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }

                EnemyRam();
                break;

            case 6:

                transform.Translate(Vector3.down * _speed * Time.deltaTime);

                if (transform.position.y < -6f)
                {
                    float randomX = Random.Range(-8f, 8f);
                    transform.position = new Vector3(randomX, 7f, 0);
                }

                if (transform.position.y <= _player.transform.position.y)
                {
                    LaserUp();
                }

                else
                {
                    LaserFire();
                }
                break;
        }
    }

    private void EnemyRed()
    {
        _isEnemyRed = true;
       
            StartCoroutine(BeamCooldownRoutine());
        
    }

    private void LaserFire()
    {
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

    private void LaserUp()
    {

        Vector3 laserOffset = new Vector3(transform.position.x, 1.5f, transform.position.z);

        if (Time.time > _canFire && _isDead == false)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, laserOffset, this.transform.rotation);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        }

    }

    public void PowerupDetected()
    {
        _powerupDetected = true;
    }
   
    private void EnemyShielded()
   
    {
        if (_isEnemyShieldDown == false)
        {
            _isEnemyShielded = true;
        }
    }

    private void EnemyRam()
    {
        if (_player != null)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < _detectRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _ramSpeed * Time.deltaTime);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyShielded == false)
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _beam.gameObject.SetActive(false);
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _ramSpeed = 0;
            _audioSource.Play();
            _isDead = true;
            
            Destroy(this.gameObject, 2.6f);

        }

        if (other.tag == "Laser" && _isEnemyShielded == false)
        {
            Destroy(other.gameObject);
            _beam.gameObject.SetActive(false);
            _isDead = true;
            
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _speed = 0;
            _ramSpeed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _spawnManager.EnemyDestroyed();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        }

        if (other.tag == "Missile" && _isEnemyShielded == false)
        {
            Destroy(other.gameObject);
            _beam.gameObject.SetActive(false);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _ramSpeed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            _isDead = true;
           
            Destroy(this.gameObject, 2.6f);
        }

        if (_isEnemyShielded == true)
        {
            _isEnemyShieldDown = true;
            _shieldEnemy.SetActive(false);
            _isEnemyShielded = false;
        }

    }

    IEnumerator BeamCooldownRoutine()
    {

        yield return new WaitForSeconds(1.5f);

        if (_isDead == false)
        {
            while (true)
            {
                {
                    _beam.gameObject.SetActive(true);
                    yield return new WaitForSeconds(2.5f);
                    _beam.gameObject.SetActive(false);
                    yield return new WaitForSeconds(5.0f);
                }
            }
        }
    }
}
