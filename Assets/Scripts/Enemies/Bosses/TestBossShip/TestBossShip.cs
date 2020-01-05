using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBossShip : Enemy {

    public float FireRate = 0.5f;
    public float SlopeMultiplier = 2.7f;
    public float BackoffRange = 4f;
    public float WanderingSpeed = 50f;

    public Transform HUDTransform;

    public Transform[] Guns = null;

    protected override void OnEnable() {
        base.OnEnable();

        UIBossHealthBar.instance.Enable();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    protected override void Attack() {
        base.Attack();

        for (int i = 0; i < Guns.Length; i++) {

            ProjectilePool.instance.RequestObject(Guns[i].position, Guns[i].rotation);
        }
    }

    public override void TakeDamage(float dmg) {
        base.TakeDamage(dmg);

        UIBossHealthBar.instance.UpdateHealth(currentHealth, MaxHealth);

        Debug.Log($"Boss has been hit for {dmg}");
    }

    protected override void Die() {
        base.Die();

        UIBossHealthBar.instance.Disable();

        Destroy(gameObject);
    }

    public override void Dispose() {

        Destroy(gameObject);
    }

    float lastAttack;

    protected override void Update() {
        
        if (lastAttack < Time.time && distanceToPlayer < AttackRange) {

            lastAttack = Time.time + FireRate;

            Attack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        WeakPoint wp = collision.otherCollider.GetComponent<WeakPoint>();

        if (wp != null) {

            float dmg = collision.relativeVelocity.magnitude * wp.WeakPointDamageMultiplier * Player.instance.DamageMultiplier;

            wp.TakeDamage(dmg);

            TakeDamage(dmg);
        }
    }

    void move() {

        if (Player.instance.gameObject.activeSelf) {

            float speed = SlopeMultiplier * Mathf.Pow(distanceToPlayer - BackoffRange, 3f);

            rigidbody.AddForce(transform.up * speed * Time.fixedDeltaTime);
        }
        else {

            rigidbody.AddForce(transform.up * WanderingSpeed * Time.fixedDeltaTime);
        }

        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, 25f);
    }
}
