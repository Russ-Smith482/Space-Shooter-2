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
    private float _regularSpeed = 3.5f;
    [SerializeField]
    private float _thrusterSpeed = 7.0f;

    [SerializeField]
    private float _thrusterBoost = 100.0f;
    [SerializeField]
    private float _BoostUsage = 200.0f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _scattershotPrefab;
    [SerializeField]
    private GameObject _missilePrefab;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;
    private float _missileCanFire = -1.0f;
    [SerializeField]
    private float _missileRate = 0.10f;


    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private CameraShake _cameraShake;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _isScattershotActive = false;
    private bool _isMissileActive = false;
    private bool _isThrusterBoostActive = false;
    private bool _isEngineStalled = false;


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
    private int _maxAmmo = 15;
    [SerializeField]
    private int _lives = 3;

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
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_cameraShake == null)
        {
            Debug.LogError("Camera Shake is NULL.");
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

        ThrusterBoost();

        if (_ammoCount > 0 && Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            PickWeapon();
        }

    }

    public void PickWeapon()
    {

        {
            if (_isTripleShotActive == true)
            {
                FireTripleShot();
            }

            else if (_isScattershotActive == true)
            {
                FireScattershot();
            }

            else if (_isMissileActive == true)
            {
                FireMissile();
            }

            else if (_isTripleShotActive == false && _isMissileActive == false && _isScattershotActive == false)
            {
                FireLaser();
            }

        }

    }
    void CalculateMovement()
    {
        if (_isEngineStalled == false)
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
    }

    void FireLaser()
    {

        _canFire = Time.time + _fireRate;

        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);

        _audioSource.Play();

        _ammoCount--;
        _uiManager.UpdateAmmo(_ammoCount);


    }
    void FireMissile()
    {
        _missileCanFire = Time.time + _missileRate;

        Instantiate(_missilePrefab, transform.position, Quaternion.identity);

        _ammoCount--;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    void FireScattershot()
    {
        _canFire = Time.time + _fireRate;

        Instantiate(_scattershotPrefab, transform.position, Quaternion.identity);

        _audioSource.Play();

        _ammoCount--;
        _uiManager.UpdateAmmo(_ammoCount);

    }

    void FireTripleShot()
    {
        _canFire = Time.time + _fireRate;

        Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

        _audioSource.Play();

        _ammoCount--;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    void ThrusterBoost()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKeyDown(KeyCode.LeftShift) && _thrusterBoost > 0)

        {
            StartCoroutine(ThrusterActive());

        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))

        {
            StartCoroutine(ThrusterReset());
        }
    }

    IEnumerator ThrusterActive()
    {
        _isThrusterBoostActive = true;
        _speed = _thrusterSpeed;

        while (Input.GetKey(KeyCode.LeftShift) && _thrusterBoost > 0)
        {
            yield return null;
            _thrusterBoost -= _BoostUsage * Time.deltaTime;
            _uiManager.UpdateThrusterBoost(_thrusterBoost);
        }
    }

    IEnumerator ThrusterReset()
    {
        _isThrusterBoostActive = false;
        _speed = _regularSpeed;

        if (_thrusterBoost < 100 && _isThrusterBoostActive == false)
        {
            yield return new WaitForSeconds(5.0f);
        }
        while (_thrusterBoost < 100 && _isThrusterBoostActive == false)

        {
            yield return null;

            _thrusterBoost += _BoostUsage * Time.deltaTime;
            _uiManager.UpdateThrusterBoost(_thrusterBoost);
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
        StartCoroutine(_cameraShake.CameraShakeCoroutine(0.4f, 0.4f));

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

    public void ScattershotActive()
    {
        _isScattershotActive = true;
        StartCoroutine(ScattershotPowerDownRoutine());
    }
    IEnumerator ScattershotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isScattershotActive = false;
    }

    public void MissileActive()
    {
        this.gameObject.transform.parent = null;
        _isMissileActive = true;
        StartCoroutine(MissilePowerDownRoutine());
    }

    IEnumerator MissilePowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isMissileActive = false;
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

    public void EngineFail()
    {
        StartCoroutine(_cameraShake.CameraShakeCoroutine(0.4f, 0.4f));
        _isEngineStalled = true;
        StartCoroutine(EngineFailPowerDownRoutine());
    }

    IEnumerator EngineFailPowerDownRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        _isEngineStalled = false;
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    public void ResetAmmoCount()

    {
        if (_ammoCount != 15)
        {
            _ammoCount = _maxAmmo;
            _uiManager.AmmoReset();
        }
    }
    public void PlayerAddLife()
    {
        if (_lives < 3)
        {

            _lives += 1;

            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);

            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
                _rightEngine.SetActive(false);
            }
            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
                _leftEngine.SetActive(true);
            }

            _uiManager.UpdateLives(_lives);
        }
    }
  
}



