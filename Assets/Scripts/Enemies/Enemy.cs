using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float MaxHealth = 100;
    public float AttackDamage = 30f;
    public float AttackRange = 7.5f;
    public float RotationSmoothing = 0.5f;

    public int ScoreValue = 0;

    protected float currentHealth;
    protected float distanceToPlayer;

    protected Vector3 pointOfInterest;

#if UNITY_EDITOR
    [SerializeField]
    bool DEBUG_Fight = true;
#endif
    [SerializeField]
    float DEBUG_newRandomDestinationTime = 2f;

    [SerializeField]
    protected AudioClip[] Sounds;

    new protected Rigidbody2D rigidbody;

    protected Action<Enemy> enemyDeathCallback;

    protected virtual void OnEnable() {

        currentHealth = MaxHealth;

        rigidbody = rigidbody == null ? GetComponent<Rigidbody2D>() : rigidbody;

        if (rigidbody == null) Debug.LogError("Enemy::Start() => No Rigidbody found!!!");
    }

    protected virtual void FixedUpdate() {

        if (Player.instance.gameObject.activeSelf == true) pointOfInterest = Player.instance.transform.position;
        else pointOfInterest = getRandomPointOfInterest();

        lookAtPointOfInterest();

        calculateDistanceToPlayer();
    }

    protected virtual void Attack() {
#if UNITY_EDITOR
        if (DEBUG_Fight == false) return;
#endif
    }

    protected virtual void TakeDamage(float dmg) {

        currentHealth = (currentHealth - dmg) < 0f ? 0f : currentHealth - dmg;

        if (currentHealth == 0f) {

            Die();
        }
    }

    protected virtual void Die() {

        if (enemyDeathCallback != null) enemyDeathCallback(this);

        if (Sounds.Length > 0) {

            SoundManager.instance.PlaySFXClip(Sounds[1]);
        }
    }

    protected virtual void OnDrawGizmosSelected() {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    protected abstract void Update();

    public void RegisterOnDeathCallback(Action<Enemy> cb) {

        enemyDeathCallback += cb;
    }

    public void UnregisterOnDeathCallback(Action<Enemy> cb) {

        enemyDeathCallback -= cb;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        Debug.Log("Collision");

        if (collision.transform.tag == "Player") {

            TakeDamage(collision.relativeVelocity.magnitude);
        }

        if (Sounds.Length > 0) {

            SoundManager.instance.PlaySFXClip(Sounds[0]);
        }

        rigidbody.AddForce(collision.relativeVelocity, ForceMode2D.Impulse);
    }

    private void calculateDistanceToPlayer() {

        if (Player.instance != null) distanceToPlayer = Vector3.Distance(transform.position, Player.instance.transform.position);
        else distanceToPlayer = Mathf.Infinity;
    }

    private void lookAtPointOfInterest() {

        Vector3 dir = (pointOfInterest - transform.position).normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f), Time.deltaTime * RotationSmoothing);
    }

    float currentPointOfInterestTime;

    Vector3 getRandomPointOfInterest() {

        if (currentPointOfInterestTime > Time.time) return pointOfInterest;

        float randomDistanceMultiplier = UnityEngine.Random.Range(10f, 50f);
        float randomTheta = UnityEngine.Random.Range(0f, 2 * Mathf.PI);

        float newX = Mathf.Cos(randomTheta);
        float newY = Mathf.Sin(randomTheta);

        currentPointOfInterestTime = Time.time + DEBUG_newRandomDestinationTime;

        return new Vector3(newX, newY) * randomDistanceMultiplier;
    }
}
