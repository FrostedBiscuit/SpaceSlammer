﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpaceShip : Enemy {

    public float BackoffRange = 1.5f;
    public float FireRate = 1f;
    public float Speed = 20f;

    [SerializeField]
    GameObject Projectile = null;

    [SerializeField]
    Transform ProjectileSpawnPoint = null;

    float nextFire;

    protected override void Attack() {
        base.Attack();

        ObjectPool.instance.RequestObject(Projectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);

        nextFire = FireRate + Time.time;
    }

    protected override void Die() {
        base.Die();

        //Debug.Log("Fighter space ship has died!!!");

        ObjectPool.instance.ReturnObject(gameObject);
    }

    protected override void OnEnable() {
        base.OnEnable();

        if (Projectile == null) {
            Debug.LogError("BasicSpaceShip::Start() => No projectile game object found!!!");
        }

        if (ProjectileSpawnPoint == null) {
            Debug.LogError("BasicSpaceShip::Start() => No projectile spawn point found!!!");
        }

        nextFire = Time.time + FireRate;
    }

    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, BackoffRange);

        if (rigidbody == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, rigidbody.velocity);
    }

    public override void TakeDamage(float dmg) {
        base.TakeDamage(dmg);

        //Debug.Log("BasicSpaceShip::TakeDamage() => Damage taken: " + dmg);
    }

    protected override void Update() {

        if (nextFire < Time.time                          && 
            Player.instance.gameObject.activeSelf == true && 
            distanceToPlayer < AttackRange) {
            Attack();
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    void move() {

        if (Player.instance.gameObject.activeSelf == true) {
            if (distanceToPlayer > AttackRange) {
                rigidbody.AddForce(transform.up * Speed * Time.fixedDeltaTime);
            }
            else if (distanceToPlayer < AttackRange && distanceToPlayer > BackoffRange) {
                rigidbody.AddForce(transform.up * Speed * Time.fixedDeltaTime * (distanceToPlayer / AttackRange));
            }
            else if (distanceToPlayer < BackoffRange) {
                rigidbody.AddForce(transform.up * Speed * Time.fixedDeltaTime * (AttackRange / distanceToPlayer) * -1f);
            }
        }
        else {
            rigidbody.AddForce(transform.up * Speed * Time.fixedDeltaTime);
        }

        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, Speed);
    }
}
