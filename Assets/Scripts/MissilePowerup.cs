using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePowerup : MonoBehaviour
{
    private float _speed = 5.0f;
    

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
                player.MissileActive();
            }
        }
        Destroy(this.gameObject);
    }
}
