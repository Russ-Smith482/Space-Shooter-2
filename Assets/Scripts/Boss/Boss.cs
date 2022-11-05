using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;


    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _leftTurret;
    [SerializeField]
    private GameObject _rightTurret;
    [SerializeField]
    private GameObject _leftTurretDestoryed;
    [SerializeField]
    private GameObject _rightTurretDestoryed;
    [SerializeField]
    private GameObject _bossDestroyedBottom;
    [SerializeField]
    private GameObject _bossDestoryedTop;

    private float _fireRate = 3f;
    private float _canFire = -1;

    [SerializeField]
    private int _bossLives = 30;

    private bool _isFullStopActive = false;
    private bool _bossDead = false;

    private UIManager _uiManager;

    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_bossDead == false)
        {
            LaserFire();
        }
        if (_isFullStopActive == false);
        {
            BossMovement();
        }
    }

    private void BossMovement()
    {

        transform.Translate((Vector3.down) * _speed * Time.deltaTime);

        if (transform.position.y <= 3.0f)
        {
            transform.Translate((Vector3.right - Vector3.down) * _speed * Time.deltaTime);

        }

        if (transform.position.x >= 0.0f)
        {
            _speed = 0;
            _isFullStopActive = true;
        }
    }

    private void LaserFire()
    {
        Vector3 laserOffset = new Vector3(transform.position.x, - 2.7f, transform.position.z);

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(2f, 6f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, laserOffset, this.transform.rotation);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

        if(transform.position.y <= -8)
        {
            Destroy(this.gameObject);
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            BossLives();
        }
    }

    public void BossLives()
    {
        if (_bossLives > 0)
        {
            _bossLives -= 1;
            _uiManager.UpdateBossLives(_bossLives);
        }

        else
        {
            _bossDead = true;
            _bossDestoryedTop.SetActive(true);
            _bossDestroyedBottom.SetActive(true);
            _uiManager.YouWin();


            Destroy(this.gameObject, 5.0f);

        }
    }

public void LeftTurretDestroyed()
    {
        _leftTurretDestoryed.SetActive(true);
    }


    public void RightTurretDestroyed()
    {
        _rightTurretDestoryed.SetActive(true);
    }

}
