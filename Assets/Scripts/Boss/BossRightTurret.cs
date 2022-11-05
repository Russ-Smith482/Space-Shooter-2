using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRightTurret : MonoBehaviour
{
    
    [SerializeField]
    private int _turretsLives = 5;
    [SerializeField]
    private GameObject _bossMissilePrefab;

    private float _fireRate = 2f;
    private float _canFire = -1;

    private AudioSource _audioSource;
    private Player _player;
    private Boss _boss;

    // Start is called before the first frame update
    void Start()

    {
        _audioSource = GetComponent<AudioSource>();
        _boss = transform.parent.GetComponent<Boss>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        if (_boss == null)
        {
            Debug.LogError("Boss is NULL.");
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            Fire();
        }
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && _turretsLives >= 1)
        {
            Destroy(other.gameObject);
            _audioSource.Play();
            _turretsLives -= 1;
        }

        else if (_turretsLives < 1)
        {
            _boss.RightTurretDestroyed();
            Destroy(this.gameObject);
        }


    }

    private void Fire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(4f, 6f);
            _canFire = Time.time + _fireRate;
            Instantiate(_bossMissilePrefab, transform.position, this.transform.rotation);
        }
    }

}
