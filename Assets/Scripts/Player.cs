using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _boostSpeedMultiplier = 2f;
    [SerializeField]
    private float _thrusterSpeed = 7.0f;
    [SerializeField]
    private float _regularSpeed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _shields = 3;
    [SerializeField]
    private GameObject _shieldMid;
    [SerializeField]
    private GameObject _shieldWeak;


    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private int _score;
    [SerializeField]
    private int _ammoCount = 15;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        ThrusterSpeed();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -5f)
        {
            transform.position = new Vector3(transform.position.x, -5f, 0);
        }

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_ammoCount > 0)
        {


            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
            }

            _audioSource.Play();

            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
        }

    }

    void ThrusterSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _thrusterSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _regularSpeed;
        }
    }

    public void Damage()
    {
        if (_isShieldActive == false)

            PlayerDamage();

        else if

            (_isShieldActive == true)

            ShieldDamage();
    }

    public void PlayerDamage()
    {
            _lives -= 1;
        
            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
            }
            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }

            _uiManager.UpdateLives(_lives);

            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        
    }

    public void ShieldDamage()
    {
            _shields -= 1;


        if (_shields == 2)
        {
            _shieldVisualizer.SetActive(false);
            _shieldMid.SetActive(true);
        }

        else if (_shields == 1)
        {
            _shieldMid.SetActive(false);
            _shieldWeak.SetActive(true);
        }

        if (_shields < 1)
        {
            _shieldWeak.SetActive(false);
            _isShieldActive = (false);

        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
      
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _boostSpeedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _boostSpeedMultiplier;
    }
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shields = 3;
        _shieldMid.SetActive(false);
        _shieldWeak.SetActive(false);
        _shieldVisualizer.SetActive(true);
       
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    
    

}
