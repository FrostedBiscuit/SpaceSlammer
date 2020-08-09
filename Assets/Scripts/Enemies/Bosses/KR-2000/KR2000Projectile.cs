using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KR2000Projectile : MonoBehaviour, IDisposable {

    public float DestroyAfter = 3f;
    public float Speed = 10f;

    public AudioClip[] ShootSounds;
    public AudioClip[] ImpactSounds;

    [HideInInspector]
    public float Damage;

    // Start is called before the first frame update
    void Start() 
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * Speed, ForceMode2D.Force);

        if (ShootSounds.Length > 0) 
        {
            var randomShootSoundIndex = Random.Range(0, ShootSounds.Length);

            SoundManager.instance.PlayRemoteSFXClip(ShootSounds[randomShootSoundIndex], transform.position);
        }

        StartCoroutine(destroyAfter(DestroyAfter));
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.tag == "Player") 
        {
            Player.instance.TakeDamage(Damage);
        }

        if (collider.isTrigger == false) 
        {
            if (ImpactSounds != null) 
            {
                var randomImpactIndex = Random.Range(0, ImpactSounds.Length);

                SoundManager.instance.PlayRemoteSFXClip(ImpactSounds[randomImpactIndex], transform.position);
            }

            StopAllCoroutines();

            Destroy(gameObject);
        }
    }

    IEnumerator destroyAfter(float time) 
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }

    public void Dispose() 
    {
        StopAllCoroutines();

        Destroy(gameObject);
    }
}
