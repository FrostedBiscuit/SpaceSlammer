using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float Speed = 10f;
    [SerializeField]
    float Damage = 20f;

    [SerializeField]
    AudioClip ShootSound = null;
    [SerializeField]
    AudioClip ImpactSound = null;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * Speed;

        SoundManager.instance.PlayRemoteSFXClip(ShootSound, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "Player") {
            Player.instance.TakeDamage(Damage);
        }

        if (collider.isTrigger == false) {

            SoundManager.instance.PlayRemoteSFXClip(ImpactSound, transform.position);

            ObjectPool.instance.ReturnObject(gameObject);
        }
    }
}
