using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Singelton
    public static Player instance;

    private void Awake() {

        if (instance != null)
            Debug.LogError("Player::Awake() => More than 1 player in the scene!!!");

        instance = this;
    }
    #endregion

    public float Speed = 10f;
    public float FollowSpeed = 1.5f;
    public float MaxHealth = 50f;
    public float ParticleInterval = 1.25f;

    [SerializeField]
    Rigidbody2D Rigidbody = null;

    [HideInInspector]
    public float Health;
    [HideInInspector]
    public float DamageMultiplier = 1f;

    [HideInInspector]
    public bool CanTakeDamage = true;

    // Start is called before the first frame update
    void OnEnable() {
        if (Rigidbody == null) {
            Debug.LogError("Player::Start() => No rigidbody reference found!!");
        }

        Health = MaxHealth;

        DamageMultiplier = 1f;

        CanTakeDamage = true;

        Rigidbody.velocity = Vector2.zero;
    }

    float lastGreatestVelocityMag;

    //private void Update() {

    //    if (lastGreatestVelocityMag < Rigidbody.velocity.magnitude) {

    //        lastGreatestVelocityMag = Rigidbody.velocity.magnitude;

    //        Debug.Log($"Greatest velocity so far: {lastGreatestVelocityMag}");
    //    }
    //}

    private void FixedUpdate() {

        Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, 80f);
    }

    public void TakeDamage(float dmg) {

        if (!CanTakeDamage) {
            return;
        }

        if (Health <= dmg) {

            Health = 0;

            ExplosionParticlesPool.instance.RequestObject(transform.position, transform.rotation);

            PlayerManager.instance.DespawnPlayer();
        }
        else {
            Health -= dmg;
        }
    }

    public Rigidbody2D GetRigidbody() {

        return Rigidbody;
    }

    float nextParticleTime;

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.transform.tag == "Enemy" && Time.time >= nextParticleTime) {

            Enemy e = collision.transform.GetComponent<Enemy>();

            if (e.Damagable == true) {
                e.TakeDamage(collision.relativeVelocity.magnitude);
            }

            CollisionParticlesPool.instance.RequestObject((Vector3)collision.GetContact(0).point, Quaternion.identity);

            nextParticleTime = Time.time + ParticleInterval;
        }
    }
}
