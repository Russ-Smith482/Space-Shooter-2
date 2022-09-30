using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{

    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private int collectable; //0 = Ammo, 1 = Health
    [SerializeField]
    // Start is called before the first frame update
    

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

                switch (collectable)
                {
                    case 0:
                       player.ResetAmmoCount();
                        break;
                    case 1:
                        player.PlayerAddLife();
                        break;
                }

            }

            AudioSource.PlayClipAtPoint(_clip, transform.position,10f);

            Destroy(this.gameObject);
        }

        else if (other.tag == "Laser")
        {
            Destroy(this.gameObject);
        }
    }

}




