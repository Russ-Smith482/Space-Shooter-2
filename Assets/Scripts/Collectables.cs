using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    
        {
           
        }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

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

            if (player != null)

            {
                player.ResetAmmoCount();
            }

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            Destroy(this.gameObject);
        }
    }

}




