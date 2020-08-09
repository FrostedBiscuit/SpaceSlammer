using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpaceShipProjectile : MonoBehaviour, IDisposable
{
    [SerializeField]
    float Speed = 10f;
    [SerializeField]
    float Damage = 20f;
    [SerializeField]
    float DestroyAfter = 5f;

    [SerializeField]
    AudioClip[] ShootSounds = null;
    [SerializeField]
    AudioClip[] ImpactSounds = null;

    public void Dispose() {

        StopAllCoroutines();

        ProjectilePool.instance.ReturnObject(this);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * Speed, ForceMode2D.Force);

        if (ShootSounds.Length > 0)
        {
            var randomShootSoundIndex = Random.Range(0, ShootSounds.Length);

            SoundManager.instance.PlayRemoteSFXClip(ShootSounds[randomShootSoundIndex], transform.position);
        }

        StartCoroutine(SelfDestruct(DestroyAfter));
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "Player") {
            Player.instance.TakeDamage(Damage);
        }

        if (collider.isTrigger == false) {

            if (ImpactSounds.Length > 0)
            {
                var randomImpactSoundIndex = Random.Range(0, ImpactSounds.Length);

                SoundManager.instance.PlayRemoteSFXClip(ImpactSounds[randomImpactSoundIndex], transform.position);
            }

            StopCoroutine(SelfDestruct(DestroyAfter));

            ProjectilePool.instance.ReturnObject(this);
        }
    }

    IEnumerator SelfDestruct(float time) {

        yield return new WaitForSeconds(time);

        ProjectilePool.instance.ReturnObject(this);
    } 
}
