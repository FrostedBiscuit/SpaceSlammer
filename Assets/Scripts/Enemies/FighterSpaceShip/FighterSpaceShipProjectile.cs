using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpaceShipProjectile : MonoBehaviour
{
    [SerializeField]
    float Speed = 10f;
    [SerializeField]
    float Damage = 20f;
    [SerializeField]
    float DestroyAfter = 5f;

    [SerializeField]
    AudioClip ShootSound = null;
    [SerializeField]
    AudioClip ImpactSound = null;

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * Speed, ForceMode2D.Force);

        SoundManager.instance.PlayRemoteSFXClip(ShootSound, transform.position);

        StartCoroutine(SelfDestruct(DestroyAfter));
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "Player") {
            Player.instance.TakeDamage(Damage);
        }

        if (collider.isTrigger == false) {

            SoundManager.instance.PlayRemoteSFXClip(ImpactSound, transform.position);

            StopCoroutine(SelfDestruct(DestroyAfter));

            ProjectilePool.instance.ReturnObject(this);
        }
    }

    IEnumerator SelfDestruct(float time) {

        yield return new WaitForSeconds(time);

        ProjectilePool.instance.ReturnObject(this);
    } 
}
