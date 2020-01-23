using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AgerrothRocket : MonoBehaviour {

    public float BoosterDelay = 3f;
    public float BoosterForce = 500f;
    public float DestroyAfter = 3f;
    public float ExplosionRadius = 3f;
    public float RotationSmoothing = 2f;

    public AudioClip BoosterSFX = null;

    public ParticleSystem ExhaustParticles = null;

    public AudioSource audioSource = null;

    [HideInInspector]
    public float Damage = 40f;

    new Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start() {

        rigidbody = GetComponent<Rigidbody2D>();

        StartCoroutine(startBoosters());
    }

    bool lookAtPlayer = true;

    private void FixedUpdate() {

        if (Player.instance == null || lookAtPlayer == false) {
            return;
        }

        Vector3 dir = (Player.instance.transform.position - transform.position).normalized;

        rigidbody.MoveRotation(Mathf.LerpAngle(rigidbody.rotation, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f, Time.deltaTime * RotationSmoothing));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.isTrigger == false) {
            explode();
        }
    }

    private void OnDrawGizmosSelected() {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

    void explode() {

        ParticlesPool.instance.RequestObject(transform.position, Quaternion.identity);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        for (int i = 0; i < cols.Length; i++) {

            if (cols[i].name == name) {
                continue;
            }

            if (cols[i].name == "Player") {
                Player.instance.TakeDamage(Damage);
            }

            Enemy e = cols[i].GetComponent<Enemy>();

            if (e != null) {

                e.TakeDamage(Damage);
            }

            Rigidbody2D colRB = cols[i].GetComponent<Rigidbody2D>();

            if (colRB != null) {

                colRB.AddForce((cols[i].transform.position - transform.position).normalized * Damage);
            }
        }

        Destroy(gameObject);
    }
 
    IEnumerator startBoosters() {

        yield return new WaitForSeconds(BoosterDelay);

        ExhaustParticles.Play();

        rigidbody.AddForce(transform.up * BoosterForce);

        lookAtPlayer = false;

        if (SoundManager.instance.PlaySFX == true) {

            audioSource.PlayOneShot(BoosterSFX);
        }

        StartCoroutine(destroyAfter());
    }

    IEnumerator destroyAfter() {

        yield return new WaitForSeconds(DestroyAfter);

        explode();
    }
}
