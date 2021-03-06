﻿using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string Name;

    public float MaxHealth = 100;
    public float AttackDamage = 30f;
    public float AttackRange = 7.5f;
    public float RotationSmoothing = 0.5f;
    public float MaxDistanceFromPlayer = 30f;

    public int ScoreValue = 0;

    public bool LookAtPlayerOnSpawn = true;
    public bool Damagable = true;

    protected float currentHealth;
    protected float distanceToPlayer;
    protected float distanceCheckInterval = 3f;

    protected Vector3 pointOfInterest;

#if UNITY_EDITOR
    [SerializeField]
    protected bool DEBUG_Fight = true;
#endif
    [SerializeField]
    float DEBUG_newRandomDestinationTime = 2f;

    [SerializeField]
    protected EnemySounds Sounds;

    new protected Rigidbody2D rigidbody;

    protected Action<Enemy> enemyDeathCallback;

    protected UIEnemyIndicator indicator;

    protected virtual void OnEnable() {

        currentHealth = MaxHealth;

        rigidbody = rigidbody == null ? GetComponent<Rigidbody2D>() : rigidbody;

        if (rigidbody == null) Debug.LogError("Enemy::Start() => No Rigidbody found!!!");

        indicator = UIEnemyIndicatorPool.instance.RequestObject();
        indicator.SetTarget(transform);

        InvokeRepeating("CheckForReposition", distanceCheckInterval, distanceCheckInterval);

        calculateDistanceToPlayer();

        if (LookAtPlayerOnSpawn == true) {

            Vector3 dir = (pointOfInterest - transform.position).normalized;

            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
        }
    }

    protected virtual void FixedUpdate() {

        pointOfInterest = Player.instance.gameObject.activeSelf ? Player.instance.transform.position : getRandomPointOfInterest();

        lookAtPointOfInterest();

        calculateDistanceToPlayer();
    }

    protected virtual void CheckForReposition() {

        if (distanceToPlayer >= MaxDistanceFromPlayer) {
        
            float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        
            Vector3 newPos = Player.instance.transform.position +
                             new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * 
                             UnityEngine.Random.Range(EnemyManager.instance.EnemySpawnNearDist, EnemyManager.instance.EnemySpawnFarDist);
        
            transform.position = newPos;
        }
    }

    protected virtual void Attack() {
#if UNITY_EDITOR
        //Debug.Log($"{name}'s fight bool: {DEBUG_Fight}");

        if (DEBUG_Fight == false) return;
#endif
    }

    public virtual void TakeDamage(float dmg) {

        currentHealth = (currentHealth - dmg) < 0f ? 0f : currentHealth - dmg;

        if (currentHealth == 0f) {

            Die();
        }
    }

    protected virtual void Die() 
    {
        if (enemyDeathCallback != null) enemyDeathCallback(this);

        if (Sounds.HasSounds)
        { 
            SoundManager.instance.PlayRemoteSFXClip(Sounds.RandomExplosionSound, transform.position);
        }
    }

    protected void calculateDistanceToPlayer() {

        if (Player.instance != null) distanceToPlayer = Vector3.Distance(transform.position, Player.instance.transform.position);
        else distanceToPlayer = Mathf.Infinity;
    }

    protected virtual void OnDrawGizmosSelected() {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    protected virtual void OnDisable() {

        if (indicator != null) {
            UIEnemyIndicatorPool.instance.ReturnObject(indicator);
        }

        CancelInvoke();
    }

    public abstract void Dispose();

    protected abstract void Update();

    public void RegisterOnDeathCallback(Action<Enemy> cb) {

        enemyDeathCallback += cb;
    }

    public void UnregisterOnDeathCallback(Action<Enemy> cb) {

        enemyDeathCallback -= cb;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (Sounds.HasSounds && SoundManager.instance.PlaySFX == true) {

            SoundManager.instance.PlayRemoteSFXClip(Sounds.RandomImpactSound, transform.position);
        }

        rigidbody.AddForce(collision.relativeVelocity, ForceMode2D.Impulse);
    }

    private void lookAtPointOfInterest() {

        Vector3 dir = (pointOfInterest - transform.position).normalized;

        rigidbody.MoveRotation(Mathf.LerpAngle(rigidbody.rotation, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f, Time.deltaTime * RotationSmoothing));//= Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f), Time.deltaTime * RotationSmoothing);
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

    [Serializable]
    protected class EnemySounds
    {
        public bool HasSounds
        {
            get
            {
                return ImpactSounds.Length > 0 && ExplosionSounds.Length > 0;
            }
        }

        public AudioClip RandomImpactSound
        {
            get
            {
                return ImpactSounds[UnityEngine.Random.Range(0, ImpactSounds.Length)];
            }
        }

        public AudioClip RandomExplosionSound
        {
            get
            {
                return ExplosionSounds[UnityEngine.Random.Range(0, ExplosionSounds.Length)];
            }
        }

        [SerializeField]
        private AudioClip[] ImpactSounds;
        [SerializeField]
        private AudioClip[] ExplosionSounds;
    }
}
