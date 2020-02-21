using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KR2000 : Enemy {

    public float WanderingSpeed = 50f;
    public float SlopeCoefficient = 10f;
    public float RotationAngle = 90f;
    public float RotationDuration = 3f;
    public float RotationCooldown = 1.5f;
    public float FireRate = 1f;

    public Vector2 SpawnOffsetFromPlayer;

    public GameObject Projectile = null;

    public Transform[] Guns = null;

    int weakpointsActive;

    protected override void OnEnable() {
        base.OnEnable();

        weakpointsActive = Guns.Length;

        if (Projectile == null) {

            Debug.LogError("KR2000::OnEnable() => No projectile game object found!!!");

            return;
        }

        Projectile.GetComponent<KR2000Projectile>().Damage = AttackDamage;

        transform.position = Player.instance.transform.position + (Vector3)SpawnOffsetFromPlayer;

        UIBossHealthBar.instance.Enable(Name);
    }

    protected override void FixedUpdate() {

        calculateDistanceToPlayer();

        move();
    }

    protected override void Attack() {
        base.Attack();

        for (int i = 0; i < Guns.Length; i++) {

            if (Projectile != null && Guns[i] != null) {

                Instantiate(Projectile, Guns[i].position, Guns[i].rotation);
            }
        }
    }

    public override void TakeDamage(float dmg) {
        base.TakeDamage(dmg);

        UIBossHealthBar.instance.UpdateHealth(currentHealth, MaxHealth);
    }

    public override void Dispose() {

        UIBossHealthBar.instance.Disable();

        Destroy(gameObject);
    }

    protected override void Die() {
        base.Die();

        UIBossHealthBar.instance.Disable();

        ExplosionParticlesPool.instance.RequestObject(transform.position, transform.rotation);

        Destroy(gameObject);
    }

    float lastAttack = 0f;

    protected override void Update() {

        if (rotatingAroundPlayer && lastAttack < Time.time) {

            Attack();

            lastAttack = Time.time + FireRate;
        }

        if (weakpointsActive == 0) {

            Damagable = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        WeakPoint wp = collision.otherCollider.transform.GetComponent<WeakPoint>();

        if (wp != null) {

            float dmg = collision.relativeVelocity.magnitude * wp.WeakPointDamageMultiplier * Player.instance.DamageMultiplier;

            wp.TakeDamage(dmg);

            TakeDamage(dmg);

            weakpointsActive -= wp.CurrentHealth > dmg ? 0 : 1 ;
        }
    }

    bool rotatingAroundPlayer = false;

    void move() {

        if (Player.instance.gameObject.activeSelf == true && distanceToPlayer > AttackRange && rotatingAroundPlayer == false) {

            Vector2 velocityDir = (Player.instance.transform.position - transform.position).normalized;

            float moveSpeed = distanceToPlayer - AttackRange > 0f ? distanceToPlayer * SlopeCoefficient : 0f;

            rigidbody.AddForce(velocityDir * moveSpeed * Time.fixedDeltaTime);

            Debug.DrawRay(transform.position, velocityDir * moveSpeed * Time.fixedDeltaTime);
        }
        else if (distanceToPlayer <= AttackRange && rotatingAroundPlayer == false && Player.instance.gameObject.activeSelf) {

            StartCoroutine(rotateAroundPlayer());
        }
        else if (Player.instance.gameObject.activeSelf == false) {

            Vector2 velocityDir = (pointOfInterest - transform.position).normalized;

            rigidbody.AddForce(velocityDir * WanderingSpeed * Time.fixedDeltaTime);
        }
    }

    bool running = false;

    IEnumerator rotateAroundPlayer() {

        if (running) {

            yield return null;
        }

        running = true;

        Debug.Log("Rotating around player");

        rigidbody.isKinematic = true;

        rotatingAroundPlayer = true;

        Vector3 playerPos = Player.instance.transform.position;

        float startAngle = rigidbody.rotation;
        float endTime = RotationDuration + Time.time;

        while (endTime > Time.time) {

            transform.RotateAround(playerPos, transform.forward, RotationAngle * (Time.deltaTime / RotationDuration));
            
            yield return null;
        }

        rigidbody.isKinematic = false;

        rotatingAroundPlayer = false;

        Debug.Log("Done rotating");

        yield return new WaitForSeconds(RotationCooldown);

        running = false;
    }

    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();

        if (rigidbody == null) {
            
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, rigidbody.velocity * 100f);
    }
}
