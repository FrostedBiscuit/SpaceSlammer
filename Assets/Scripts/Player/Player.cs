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

    [SerializeField]
    float DisableAnimatorAfter = 1.167f;

    // Start is called before the first frame update
    void OnEnable() {
        if (Rigidbody == null) {
            Debug.LogError("Player::Start() => No rigidbody reference found!!");
        }

        Health = MaxHealth;

        DamageMultiplier = 1f;

        Rigidbody.velocity = Vector2.zero;

        Invoke("disableAnimator", DisableAnimatorAfter);
    }

    public void TakeDamage(float dmg) {

        if (Health <= dmg) {

            Health = 0;

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

            e.TakeDamage(collision.relativeVelocity.magnitude * DamageMultiplier);

            ParticlesPool.instance.RequestObject((Vector3)collision.GetContact(0).point, Quaternion.identity);

            nextParticleTime = Time.time + ParticleInterval;
        }
    }

    private void disableAnimator() {

        GetComponent<Animator>().enabled = false;
    }
}
