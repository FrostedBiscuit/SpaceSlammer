using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StunnerShipProjectile : MonoBehaviour, IDisposable {

    [SerializeField]
    float Speed = 50f;
    [SerializeField]
    float ProjectileLifetime = 2f;
    [SerializeField]
    float StunRadius = 3f;
    [SerializeField]
    float StunDuration = 3f;
    [SerializeField]
    float Damage = 15f;

    [SerializeField]
    AudioClip[] ShotSFX = null;
    [SerializeField]
    AudioClip[] ExplosionSFX = null;

    [SerializeField]
    AudioSource Source;

    bool isFlying = false;

    private void OnEnable() 
    {
        if (SoundManager.instance.PlaySFX && ShotSFX.Length > 0)
        {
            var randomShotSound = Random.Range(0, ShotSFX.Length);

            Source.PlayOneShot(ShotSFX[randomShotSound]);
        }

        isFlying = false;

        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter() 
    {
        yield return new WaitForSeconds(ProjectileLifetime);

        Explode();
    }

    public void Boost() 
    {
        isFlying = true;

        transform.parent = null;

        GetComponent<Rigidbody2D>().velocity = transform.up * Speed;
    }

    private void OnTriggerEnter2D() 
    {
        if (isFlying == false) 
        {
            return;
        }

        StopAllCoroutines();

        Explode();
    }

    private void Explode() 
    {
        if (SoundManager.instance.PlaySFX && ExplosionSFX.Length > 0)
        {
            var randomExplosionSFX = Random.Range(0, ExplosionSFX.Length);

            SoundSourcePool.instance.RequestObject(transform.position, transform.rotation).GetComponent<SoundSource>().Play(ExplosionSFX[randomExplosionSFX]);
        }

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, StunRadius);

        for (int i = 0; i < cols.Length; i++) 
        {
            if (cols[i].tag == "Player") 
            {
                Player.instance.TakeDamage(Damage);

                CameraShake.instance.Shake();

                PlayerManager.instance.ApplyEffect(PlayerManager.Effect.STUN, duration: StunDuration);
            }
            else if (cols[i].tag == "Enemy")
            {
                cols[i].transform.GetComponent<Enemy>().TakeDamage(Damage);
            }
        }

        // TODO: make and spawn explosion partilces

        transform.SetParent(StunnerShipProjectilePool.instance.transform);

        StunnerShipProjectilePool.instance.ReturnObject(this);
    }

    public void Dispose() 
    {
        StopAllCoroutines();

        StunnerShipProjectilePool.instance.ReturnObject(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, StunRadius);
    }
}
