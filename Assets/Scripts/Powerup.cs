using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private float _dragSpeed = 5f;
    [SerializeField]
    private float _detectRange = 1.5f;


    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = Shields, 3 = Scatter, 4 =  BlackHole
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
    }

        // Update is called once per frame
        void Update()
    {
     
        if (Vector3.Distance(_player.transform.position, transform.position) < _detectRange && Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _dragSpeed * Time.deltaTime);
        }

        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
           
            if (player != null)
            {
               switch(powerupID)
               {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive(); 
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.ScattershotActive();
                        break;
                    case 4:
                        player.EngineFail();
                        break;
               }
            }
            
            Destroy(this.gameObject);
        }

        else if (other.tag == "Laser")
        {
            Destroy(this.gameObject);
        }
    }
 
}

