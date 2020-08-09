using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AgerrothRocket : MonoBehaviour, IDisposable {

    public float BoosterDelay = 3f;
    public float BoosterForce = 500f;
    public float DestroyAfter = 3f;
    public float ExplosionRadius = 3f;
    public float RotationSmoothing = 2f;

    public AudioClip[] BoosterSFX;
    public AudioClip[] ExplosionsSFX;

    public ParticleSystem ExhaustParticles = null;

    public AudioSource audioSource = null;

    [HideInInspector]
    public float Damage = 40f;

    new Rigidbody2D rigidbody;

    private bool agerrothProtected = true;

    // Start is called before the first frame update
    void Start() 
    {
        rigidbody = GetComponent<Rigidbody2D>();

        StartCoroutine(startBoosters());
    }

    bool flying = false;

    private void FixedUpdate()
    { 

        if (Player.instance == null) 
        {
            return;
        }

        Vector3 dir = (Player.instance.transform.position - transform.position).normalized;

        rigidbody.MoveRotation(Mathf.LerpAngle(rigidbody.rotation, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f, Time.fixedDeltaTime * RotationSmoothing));

        if (flying == true) 
        {
            rigidbody.AddForce(transform.up * BoosterForce);
        }

        RotationSmoothing = Mathf.Lerp(RotationSmoothing, 0f, Time.fixedDeltaTime / (BoosterDelay + DestroyAfter));
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.isTrigger == false && (collision.transform.GetComponent<Agerroth>() == null || agerrothProtected == false)) 
        {
            explode();
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

    void explode() 
    {
        CollisionParticlesPool.instance.RequestObject(transform.position, Quaternion.identity);

        if (SoundManager.instance.PlaySFX) 
        {
            int randomExplosionSFXIndex = Random.Range(0, ExplosionsSFX.Length);

            SoundSourcePool.instance.RequestObject(transform.position, transform.rotation).Play(ExplosionsSFX[randomExplosionSFXIndex]);
        }

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        for (int i = 0; i < cols.Length; i++) 
        {
            if (cols[i].name == name) 
            {
                continue;
            }

            if (cols[i].tag == "Player") 
            {
                Player.instance.TakeDamage(Damage);
            }

            Enemy e = cols[i].GetComponent<Enemy>();

            if (e != null) 
            {
                e.TakeDamage(Damage);
            }

            Rigidbody2D colRB = cols[i].GetComponent<Rigidbody2D>();

            if (colRB != null) 
            {
                colRB.AddForce((cols[i].transform.position - transform.position).normalized * Damage);
            }
        }

        Destroy(gameObject);
    }
 
    IEnumerator startBoosters() 
    {
        yield return new WaitForSeconds(BoosterDelay);

        ExhaustParticles.Play();

        flying = true;

        if (SoundManager.instance.PlaySFX == true && BoosterSFX.Length > 0) 
        {
            var randomBoosterSFX = Random.Range(0, BoosterSFX.Length);

            audioSource.PlayOneShot(BoosterSFX[randomBoosterSFX]);
        }

        StartCoroutine(destroyAfter());
    }

    IEnumerator destroyAfter() 
    {
        yield return new WaitForSeconds(DestroyAfter / 2f);

        agerrothProtected = false;

        yield return new WaitForSeconds(DestroyAfter / 2f);

        explode();
    }

    public void Dispose() 
    {
        StopAllCoroutines();

        Destroy(gameObject);
    }
}
