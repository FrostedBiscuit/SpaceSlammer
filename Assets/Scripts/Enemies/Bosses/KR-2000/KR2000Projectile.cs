using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KR2000Projectile : MonoBehaviour, IDisposable {

    public float DestroyAfter = 3f;
    public float Speed = 10f;

    public AudioClip ShootSound;
    public AudioClip ImpactSound;

    [HideInInspector]
    public float Damage;

    // Start is called before the first frame update
    void Start() {

        GetComponent<Rigidbody2D>().AddForce(transform.up * Speed, ForceMode2D.Force);

        if (ShootSound != null) {

            SoundManager.instance.PlayRemoteSFXClip(ShootSound, transform.position);
        }

        StartCoroutine(destroyAfter(DestroyAfter));
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "Player") {

            Player.instance.TakeDamage(Damage);
        }

        if (collider.isTrigger == false) {

            if (ImpactSound != null) {

                SoundManager.instance.PlayRemoteSFXClip(ImpactSound, transform.position);
            }

            StopAllCoroutines();

            Destroy(gameObject);
        }
    }

    IEnumerator destroyAfter(float time) {

        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }

    public void Dispose() {

        StopAllCoroutines();

        Destroy(gameObject);
    }
}
